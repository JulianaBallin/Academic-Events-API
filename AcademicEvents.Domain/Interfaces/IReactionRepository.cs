using AcademicEvents.Domain.Entities;

namespace AcademicEvents.Domain.Interfaces;

/// <summary>
/// Contrato do repository de reacoes.
/// A implementacao fica na Infrastructure.
/// </summary>
public interface IReactionRepository
{
    Task<Reaction> CreateAsync(Reaction reacao);
    Task<Reaction?> GetByIdAsync(int id);
    Task<List<Reaction>> GetByEventoAsync(int eventoId);
    Task<Reaction?> GetByUsuarioEEventoAsync(int usuarioId, int eventoId);
    Task DeleteAsync(int id);
}
