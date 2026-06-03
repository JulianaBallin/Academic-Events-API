using AcademicEvents.Application.DTOs.Event;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contract of academic events service.
/// </summary>
public interface IEventService
{
    Task<EventResponse> CreateAsync(CreateEventRequest request, int organizerId);
    Task<EventResponse?> GetByIdAsync(int id);
    Task<List<EventResponse>> GetAllAsync(string? status, int? organizerId);
    Task<List<EventResponse>> GetByOrganizerAsync(int organizerId);
    Task<EventResponse?> UpdateAsync(int id, UpdateEventRequest request, int userId);
    Task DeleteAsync(int id, int userId);
}
