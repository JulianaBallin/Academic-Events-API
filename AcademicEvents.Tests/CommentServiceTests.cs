using AcademicEvents.Application.DTOs.Comment;
using AcademicEvents.Application.Services;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;
using Moq;

namespace AcademicEvents.Tests;

/// <summary>
/// Testes unitários das regras de comentário em eventos.
/// </summary>
public class CommentServiceTests
{
    private readonly Mock<ICommentRepository> _commentRepositoryMock = new();
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();

    [Fact]
    public async Task CreateAsync_EventoNaoExiste_LancaNotFoundException()
    {
        CommentService service = new CommentService(
            _commentRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateCommentRequest request = new CreateCommentRequest
        {
            EventoId = 123,
            Conteudo = "Comentário válido."
        };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(123))
            .ReturnsAsync((Event?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.CreateAsync(request, usuarioId: 1));
    }

    [Fact]
    public async Task DeleteAsync_UsuarioNaoAutor_LancaUnauthorizedException()
    {
        CommentService service = new CommentService(
            _commentRepositoryMock.Object,
            _eventRepositoryMock.Object);

        _commentRepositoryMock
            .Setup(repository => repository.GetByIdAsync(8))
            .ReturnsAsync(new Comment { Id = 8, UsuarioId = 2, EventoId = 1 });

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => service.DeleteAsync(id: 8, usuarioId: 1));
    }
}
