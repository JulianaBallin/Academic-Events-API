using AcademicEvents.Domain.Entities;

namespace AcademicEvents.Domain.Interfaces;

/// <summary>
/// Contrato do repository de inscricoes.
/// A implementacao fica na Infrastructure.
/// </summary>
public interface IRegistrationRepository
{
    Task<Registration> CreateAsync(Registration inscricao);
    Task<Registration?> GetByIdAsync(int id);
    Task<List<Registration>> GetByUsuarioAsync(int usuarioId);
    Task<List<Registration>> GetByEventoAsync(int eventoId);
    // verifica se o par usuario-evento ja existe antes de inserir
    Task<Registration?> GetByUsuarioEEventoAsync(int usuarioId, int eventoId);
    Task<Registration?> UpdateAsync(Registration inscricao);
    Task DeleteAsync(int id);
}
