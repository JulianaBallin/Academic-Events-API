using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Infrastructure.Data;
using AcademicEvents.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcademicEvents.Infrastructure;

/// <summary>
/// Extension that registers all Infrastructure layer dependencies in ASP.NET DI.
/// Called in Program.cs using builder.Services.AddInfrastructure(config).
/// </summary>
public static class InfrastructureDependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("Connection string not found.");

        services.AddDbContext<AcademicEventsDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Registers each repository as scoped, with one instance per request.
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IReactionRepository, ReactionRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
    }
}
