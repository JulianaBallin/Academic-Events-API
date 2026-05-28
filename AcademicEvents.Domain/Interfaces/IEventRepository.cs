using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Interfaces;

/// <summary>
/// Contrato do repository de eventos.
/// A implementação fica na Infrastructure.
/// </summary>
public interface IEventRepository
{
    Task<Event> CreateAsync(Event evento);
    Task<Event?> GetByIdAsync(int id);
    Task<List<Event>> GetAllAsync();
    Task<List<Event>> GetByStatusAsync(StatusEvento status);
    Task<List<Event>> GetByOrganizadorAsync(int organizadorId);
    Task<Event?> UpdateAsync(Event evento);
    Task DeleteAsync(int id);
}
