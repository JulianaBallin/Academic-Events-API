using AcademicEvents.Application.DTOs.Activity;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contrato do service das atividades de eventos acadêmicos.
/// </summary>
public interface IActivityService 
{
    Task<ActivityResponse> CreateAsync(CreateActivityRequest request);
    Task<ActivityResponse?> GetByIdAsync(int id);
    Task<List<ActivityResponse>> GetByEventIdAsync(int eventId);
}