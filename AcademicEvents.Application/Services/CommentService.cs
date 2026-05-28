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

    public CommentService(ICommentRepository repository)
    {
        _repository = repository;
    }

    public async Task<CommentResponse> CreateAsync(CreateCommentRequest request, int usuarioId)
    {
        Comment comentario = new Comment
        {
            EventoId = request.EventoId,
            UsuarioId = usuarioId,
            Conteudo = request.Conteudo
        };

        Comment criado = await _repository.CreateAsync(comentario);
        return MapearParaResponse(criado);
    }

    public async Task<List<CommentResponse>> GetByEventoAsync(int eventoId)
    {
        List<Comment> comentarios = await _repository.GetByEventoAsync(eventoId);
        return comentarios.Select(MapearParaResponse).ToList();
    }

    public async Task DeleteAsync(int id, int usuarioId)
    {
        Comment? comentario = await _repository.GetByIdAsync(id);
        if (comentario is null) throw new NotFoundException("Comentário não encontrado.");

        // só o autor pode deletar o próprio comentário
        if (comentario.UsuarioId != usuarioId)
            throw new UnauthorizedException("Apenas o autor pode remover este comentário.");

        await _repository.DeleteAsync(id);
    }

    private static CommentResponse MapearParaResponse(Comment comentario)
    {
        return new CommentResponse
        {
            Id = comentario.Id,
            EventoId = comentario.EventoId,
            UsuarioId = comentario.UsuarioId,
            NomeUsuario = comentario.Usuario?.Nome ?? string.Empty,
            Conteudo = comentario.Conteudo,
            CriadoEm = comentario.CriadoEm
        };
    }
}
