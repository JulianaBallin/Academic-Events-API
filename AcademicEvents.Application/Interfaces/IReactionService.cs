using AcademicEvents.Application.DTOs.Reaction;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contrato do service de reacoes a eventos.
/// </summary>
public interface IReactionService
{
    Task<ReactionResponse> CreateAsync(CreateReactionRequest request, int usuarioId);
    Task<List<ReactionResponse>> GetByEventoAsync(int eventoId);
    Task DeleteAsync(int id, int usuarioId);
}
