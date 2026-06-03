using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Interfaces;

/// <summary>
/// Event repository contract.
/// Implementation kept on Infrastructure.
/// </summary>
public interface IEventRepository
{
    Task<Event> CreateAsync(Event @event);
    Task<Event?> GetByIdAsync(int id);
    Task<List<Event>> GetAllAsync();
    Task<List<Event>> GetByStatusAsync(EventStatus eventStatus);
    Task<List<Event>> GetByOrganizerAsync(int organizerId);
    Task<List<Event>> GetFilteredAsync(EventStatus? status, int? organizerId);
    Task<Event?> UpdateAsync(Event @event);
    Task DeleteAsync(int id);
}
