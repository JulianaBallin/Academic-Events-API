using AcademicEvents.Application.DTOs.Comment;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Service for event comments.
/// </summary>
public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;
    private readonly IEventRepository _eventRepository;

    public CommentService(ICommentRepository repository, IEventRepository eventRepository)
    {
        _repository = repository;
        _eventRepository = eventRepository;
    }

    public async Task<CommentResponse> CreateAsync(CreateCommentRequest request, int userId)
    {
        if (request.EventId <= 0)
            throw new InvalidOperationException("Event id must be greater than zero.");

        string content = (request.Content ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Comment content is required.");

        Event? ev = await _eventRepository.GetByIdAsync(request.EventId);
        if (ev is null)
            throw new NotFoundException("Evento nao encontrado.");

        Comment comment = new Comment
        {
            EventId = request.EventId,
            UserId = userId,
            Content = content
        };

        Comment created = await _repository.CreateAsync(comment);
        return MapToResponse(created);
    }

    public async Task<List<CommentResponse>> GetByEventAsync(int eventId)
    {
        if (eventId <= 0)
            throw new InvalidOperationException("Event id must be greater than zero.");

        List<Comment> comments = await _repository.GetByEventAsync(eventId);
        return comments.Select(MapToResponse).ToList();
    }

    public async Task DeleteAsync(int id, int userId)
    {
        Comment? comment = await _repository.GetByIdAsync(id);
        if (comment is null) throw new NotFoundException("Comentario nao encontrado.");

        if (comment.UserId != userId)
            throw new UnauthorizedException("Apenas o autor pode remover este comentario.");

        await _repository.DeleteAsync(id);
    }

    private static CommentResponse MapToResponse(Comment comment)
    {
        return new CommentResponse
        {
            Id = comment.Id,
            EventId = comment.EventId,
            UserId = comment.UserId,
            UserName = comment.User?.Name ?? string.Empty,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt
        };
    }
}
