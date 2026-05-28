using AcademicEvents.Application.DTOs.Registration;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contrato do service de inscrições em eventos.
/// </summary>
public interface IRegistrationService
{
    Task<RegistrationResponse> CreateAsync(CreateRegistrationRequest request, int usuarioId);
    Task<List<RegistrationResponse>> GetByUsuarioAsync(int usuarioId);
    Task DeleteAsync(int id, int usuarioId);
}
