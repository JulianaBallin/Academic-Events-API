using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using AcademicEvents.API.Middlewares;
using AcademicEvents.Application;
using AcademicEvents.Exceptions;
using AcademicEvents.Infrastructure;
using AcademicEvents.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        List<string> messages = context.ModelState.Values
            .SelectMany(value => value.Errors)
            .Select(error => error.ErrorMessage)
            .Where(message => !string.IsNullOrWhiteSpace(message))
            .ToList();

        ErrorResponse response = new ErrorResponse
        {
            Message = messages.Count == 0
                ? "Invalid data on request."
                : string.Join(" ", messages),
            StatusCode = StatusCodes.Status400BadRequest,
            Path = context.HttpContext.Request.Path,
            UtcTime = DateTime.UtcNow
        };

        return new BadRequestObjectResult(response);
    };
});
builder.Services.AddEndpointsApiExplorer();

// Infrastructure registers the DbContext and repositories.
builder.Services.AddInfrastructure(builder.Configuration);

// Application registers the services (AuthService, EventService, etc.).
builder.Services.AddApplication();

// Configures JWT Bearer Token.
string jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                string authorization = context.Request.Headers.Authorization.ToString();

                // Accepts the raw token to avoid common errors when testing with Swagger.
                if (!string.IsNullOrWhiteSpace(authorization)
                    && !authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                    && authorization.Split('.').Length == 3)
                {
                    context.Token = authorization;
                }

                return Task.CompletedTask;
            }
        };
    });

// Configures Swagger to accept Bearer tokens through the Authorize button.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Academic Events API", Version = "v1" });

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Paste only the JWT token returned by login or registration."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Creates the database if it does not exist. Useful during development.
using (IServiceScope scope = app.Services.CreateScope())
{
    AcademicEventsDbContext context = scope.ServiceProvider
        .GetRequiredService<AcademicEventsDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
