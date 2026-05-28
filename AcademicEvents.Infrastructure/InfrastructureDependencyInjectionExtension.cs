using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Infrastructure.Data;
using AcademicEvents.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcademicEvents.Infrastructure;

/// <summary>
/// Extension que registra tudo da Infrastructure no DI do ASP.NET.
/// Chamado no Program.cs com builder.Services.AddInfrastructure(config).
/// </summary>
public static class InfrastructureDependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string não encontrada.");

        services.AddDbContext<AcademicEventsDbContext>(options =>
            options.UseNpgsql(connectionString));

        // registra cada repository como Scoped (uma instância por request)
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IReactionRepository, ReactionRepository>();
    }
}
