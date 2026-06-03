using AcademicEvents.Application.Interfaces;
using AcademicEvents.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AcademicEvents.Application;

/// <summary>
/// Registers Application layer services in ASP.NET dependency injection.
/// Call in Program.cs using builder.Services.AddApplication().
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
