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
        List<string> mensagens = context.ModelState.Values
            .SelectMany(value => value.Errors)
            .Select(error => error.ErrorMessage)
            .Where(message => !string.IsNullOrWhiteSpace(message))
            .ToList();

        ErrorResponse response = new ErrorResponse
        {
            Mensagem = mensagens.Count == 0
                ? "Dados inválidos na requisição."
                : string.Join(" ", mensagens),
            StatusCode = StatusCodes.Status400BadRequest,
            Caminho = context.HttpContext.Request.Path,
            DataHoraUtc = DateTime.UtcNow
        };

        return new BadRequestObjectResult(response);
    };
});
builder.Services.AddEndpointsApiExplorer();

// Infrastructure registra o DbContext e os repositories
builder.Services.AddInfrastructure(builder.Configuration);

// Application registra os services (AuthService, EventService, etc.)
builder.Services.AddApplication();

// configura JWT Bearer Token
string jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key não configurada.");

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

                // Aceita o token puro para evitar erro comum ao testar pelo Swagger.
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

// configura o Swagger para aceitar o token Bearer no botão Authorize
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
        Description = "Cole apenas o token JWT retornado no login ou cadastro."
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

// cria o banco se não existir (útil no desenvolvimento)
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
