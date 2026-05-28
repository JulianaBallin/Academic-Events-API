using AcademicEvents.Application.Interfaces;
using AcademicEvents.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AcademicEvents.Application;

/// <summary>
/// Registra os services da camada Application no DI do ASP.NET.
/// Chamar no Program.cs com builder.Services.AddApplication().
/// </summary>
public static class ApplicationDependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IReactionService, ReactionService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
    }
}
