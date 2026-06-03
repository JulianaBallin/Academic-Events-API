using AcademicEvents.Application.DTOs.Reaction;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Service for event reactions.
/// A user can have only one reaction per event.
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

    public async Task<ReactionResponse> CreateAsync(CreateReactionRequest request, int userId)
    {
        if (request.EventId <= 0)
            throw new InvalidOperationException("Event id must be greater than zero.");

        if (!Enum.IsDefined(request.Type))
            throw new InvalidOperationException("Invalid reaction type.");

        Event? eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
        if (eventEntity is null)
            throw new NotFoundException("Event not found.");

        // Checks whether the user has already reacted to this event.
        Reaction? existentReaction = await _repository.GetByUserEventAsync(userId, request.EventId);
        if (existentReaction is not null)
            throw new InvalidOperationException("You have already reacted to this event. Delete the previous reaction to change it.");

        Reaction reaction = new Reaction
        {
            EventId = request.EventId,
            UserId = userId,
            Type = request.Type
        };

        Reaction createdReaction = await _repository.CreateAsync(reaction);
        return MapResponse(createdReaction);
    }

    public async Task<List<ReactionResponse>> GetByEventAsync(int eventId)
    {
        if (eventId <= 0)
            throw new InvalidOperationException("Event id must be greater than zero.");

        List<Reaction> reactions = await _repository.GetByEventAsync(eventId);
        return reactions.Select(MapResponse).ToList();
    }

    public async Task DeleteAsync(int id, int userId)
    {
        Reaction? reaction = await _repository.GetByIdAsync(id);
        if (reaction is null) throw new NotFoundException("Reaction not found .");

        if (reaction.UserId != userId)
            throw new UnauthorizedException("Only author can delete this reaction.");

        await _repository.DeleteAsync(id);
    }

    private static ReactionResponse MapResponse(Reaction reaction)
    {
        return new ReactionResponse
        {
            Id = reaction.Id,
            EventId = reaction.EventId,
            UserId = reaction.UserId,
            UserName = reaction.User?.Name ?? string.Empty,
            Type = reaction.Type.ToString(),
            CreatedAt = reaction.CreatedAt
        };
    }
}
