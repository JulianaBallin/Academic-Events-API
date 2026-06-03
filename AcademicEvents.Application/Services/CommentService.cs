using AcademicEvents.Application.DTOs.Comment;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Service de comentários em eventos.
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
            throw new InvalidOperationException("O id do evento deve ser maior que zero.");

        string conteudo = (request.Content ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new InvalidOperationException("O conteúdo do comentário é obrigatório.");

        Event? evento = await _eventRepository.GetByIdAsync(request.EventId);
        if (evento is null)
            throw new NotFoundException("Evento não encontrado.");

        Comment comentario = new Comment
        {
            EventId = request.EventId,
            UserId = userId,
            Content = conteudo
        };

        Comment criado = await _repository.CreateAsync(comentario);
        return MapearParaResponse(criado);
    }

    public async Task<List<CommentResponse>> GetByEventAsync(int eventId)
    {
        if (eventId <= 0)
            throw new InvalidOperationException("O id do evento deve ser maior que zero.");

        List<Comment> comentarios = await _repository.GetByEventAsync(eventId);
        return comentarios.Select(MapearParaResponse).ToList();
    }

    public async Task DeleteAsync(int id, int userId)
    {
        Comment? comentario = await _repository.GetByIdAsync(id);
        if (comentario is null) throw new NotFoundException("Comentário não encontrado.");

        // só o autor pode deletar o próprio comentário
        if (comentario.UserId != userId)
            throw new UnauthorizedException("Apenas o autor pode remover este comentário.");

        await _repository.DeleteAsync(id);
    }

    private static CommentResponse MapearParaResponse(Comment comentario)
    {
        return new CommentResponse
        {
            Id = comentario.Id,
            EventId = comentario.EventId,
            UserId = comentario.UserId,
            UserName = comentario.User?.Name ?? string.Empty,
            Content = comentario.Content,
            CreatedAt = comentario.CreatedAt
        };
    }
}
