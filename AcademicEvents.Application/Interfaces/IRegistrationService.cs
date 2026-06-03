using AcademicEvents.Application.DTOs.Registration;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contract for the event registration service.
/// </summary>
public interface IRegistrationService
{
    Task<RegistrationResponse> CreateAsync(CreateRegistrationRequest request, int userId);
    Task<List<RegistrationResponse>> GetByUserAsync(int userId);
    Task DeleteAsync(int id, int userId);
}
