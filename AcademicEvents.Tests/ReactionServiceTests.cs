using AcademicEvents.Application.DTOs.Reaction;
using AcademicEvents.Application.Services;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Enums;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;
using Moq;

namespace AcademicEvents.Tests;

/// <summary>
/// Testes unitários das regras de reação em eventos.
/// </summary>
public class ReactionServiceTests
{
    private readonly Mock<IReactionRepository> _reactionRepositoryMock = new();
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();

    [Fact]
    public async Task CreateAsync_EventoNaoExiste_LancaNotFoundException()
    {
        ReactionService service = new ReactionService(
            _reactionRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateReactionRequest request = new CreateReactionRequest
        {
            EventoId = 404,
            Tipo = TipoReacao.Curtir
        };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(404))
            .ReturnsAsync((Event?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.CreateAsync(request, usuarioId: 1));
    }

    [Fact]
    public async Task CreateAsync_ReacaoDuplicada_LancaInvalidOperationException()
    {
        ReactionService service = new ReactionService(
            _reactionRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateReactionRequest request = new CreateReactionRequest
        {
            EventoId = 7,
            Tipo = TipoReacao.VouParticipar
        };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(7))
            .ReturnsAsync(new Event { Id = 7, OrganizadorId = 2 });

        _reactionRepositoryMock
            .Setup(repository => repository.GetByUsuarioEEventoAsync(1, 7))
            .ReturnsAsync(new Reaction { Id = 3, UsuarioId = 1, EventoId = 7 });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(request, usuarioId: 1));
    }

    [Fact]
    public async Task CreateAsync_TipoForaDoEnum_LancaInvalidOperationException()
    {
        ReactionService service = new ReactionService(
            _reactionRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateReactionRequest request = new CreateReactionRequest
        {
            EventoId = 7,
            Tipo = (TipoReacao)99
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(request, usuarioId: 1));
    }
}
