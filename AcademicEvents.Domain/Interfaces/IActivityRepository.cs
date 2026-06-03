using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Interfaces;


/// <summary>
/// Contrato do repository de atividades dos eventos.
/// A implementação fica na Infrastructure.
/// </summary>
public interface IActivityRepository
{
    Task<Activity> CreateAsync(Activity activity);

    Task<Activity?> GetByIdAsync(int id);

    Task<List<Activity>> GetByEventIdAsync(int eventId);

    Task UpdateAsync(Activity activity);

    Task DeleteAsync(Activity activity);
}