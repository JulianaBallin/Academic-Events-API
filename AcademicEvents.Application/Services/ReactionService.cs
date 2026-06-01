using AcademicEvents.Application.DTOs.Reaction;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Service de reações a eventos.
/// Um usuário pode ter apenas uma reação por evento.
/// </summary>
public class ReactionService : IReactionService
{
    private readonly IReactionRepository _repository;
    private readonly IEventRepository _eventRepository;

    public ReactionService(IReactionRepository repository, IEventRepository eventRepository)
    {
        _repository = repository;
        _eventRepository = eventRepository;
    }

    public async Task<ReactionResponse> CreateAsync(CreateReactionRequest request, int usuarioId)
    {
        if (request.EventoId <= 0)
            throw new InvalidOperationException("O id do evento deve ser maior que zero.");

        if (!Enum.IsDefined(request.Tipo))
            throw new InvalidOperationException("Tipo de reação inválido.");

        Event? evento = await _eventRepository.GetByIdAsync(request.EventoId);
        if (evento is null)
            throw new NotFoundException("Evento não encontrado.");

        // verifica se o usuário já reagiu a este evento
        Reaction? existente = await _repository.GetByUsuarioEEventoAsync(usuarioId, request.EventoId);
        if (existente is not null)
            throw new InvalidOperationException("Você já reagiu a este evento. Delete a reação anterior para mudar.");

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
        if (eventoId <= 0)
            throw new InvalidOperationException("O id do evento deve ser maior que zero.");

        List<Reaction> reacoes = await _repository.GetByEventoAsync(eventoId);
        return reacoes.Select(MapearParaResponse).ToList();
    }

    public async Task DeleteAsync(int id, int usuarioId)
    {
        Reaction? reacao = await _repository.GetByIdAsync(id);
        if (reacao is null) throw new NotFoundException("Reação não encontrada.");

        if (reacao.UsuarioId != usuarioId)
            throw new UnauthorizedException("Apenas o autor pode remover esta reação.");

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
