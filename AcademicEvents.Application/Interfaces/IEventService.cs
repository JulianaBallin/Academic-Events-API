using AcademicEvents.Application.DTOs.Event;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contrato do service de eventos acadêmicos.
/// </summary>
public interface IEventService
{
    Task<EventResponse> CreateAsync(CreateEventRequest request, int organizadorId);
    Task<EventResponse?> GetByIdAsync(int id);
    Task<List<EventResponse>> GetAllAsync(string? status);
    Task<EventResponse?> UpdateAsync(int id, UpdateEventRequest request, int usuarioId);
    Task DeleteAsync(int id, int usuarioId);
}
