using AcademicEvents.Application.DTOs.Reaction;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Service de reacoes a eventos.
/// Um usuario pode ter apenas uma reacao por evento.
/// </summary>
public class ReactionService : IReactionService
{
    private readonly IReactionRepository _repository;

    public ReactionService(IReactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReactionResponse> CreateAsync(CreateReactionRequest request, int usuarioId)
    {
        // verifica se o usuario ja reagiu a este evento
        Reaction? existente = await _repository.GetByUsuarioEEventoAsync(usuarioId, request.EventoId);
        if (existente is not null)
            throw new InvalidOperationException("Voce ja reagiu a este evento. Delete a reacao anterior para mudar.");

        Reaction reacao = new Reaction
        {
            EventoId = request.EventoId,
            UsuarioId = usuarioId,
            Tipo = request.Tipo
        };

        Reaction criada = await _repository.CreateAsync(reacao);
        return MapearParaResponse(criada);
    }

    public async Task<List<ReactionResponse>> GetByEventoAsync(int eventoId)
    {
        List<Reaction> reacoes = await _repository.GetByEventoAsync(eventoId);
        return reacoes.Select(MapearParaResponse).ToList();
    }

    public async Task DeleteAsync(int id, int usuarioId)
    {
        Reaction? reacao = await _repository.GetByIdAsync(id);
        if (reacao is null) throw new NotFoundException("Reacao nao encontrada.");

        if (reacao.UsuarioId != usuarioId)
            throw new UnauthorizedException("Apenas o autor pode remover esta reacao.");

        await _repository.DeleteAsync(id);
    }

    private static ReactionResponse MapearParaResponse(Reaction reacao)
    {
        return new ReactionResponse
        {
            Id = reacao.Id,
            EventoId = reacao.EventoId,
            UsuarioId = reacao.UsuarioId,
            NomeUsuario = reacao.Usuario?.Nome ?? string.Empty,
            Tipo = reacao.Tipo.ToString(),
            CriadoEm = reacao.CriadoEm
        };
    }
}
