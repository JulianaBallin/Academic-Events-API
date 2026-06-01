using AcademicEvents.Application.DTOs.Event;
using AcademicEvents.Application.Services;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;
using Moq;

namespace AcademicEvents.Tests;

/// <summary>
/// Testes unitários das regras principais do EventService.
/// </summary>
public class EventServiceTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();

    [Fact]
    public async Task CreateAsync_DataFimAntesDaDataInicio_LancaInvalidOperationException()
    {
        EventService service = new EventService(_eventRepositoryMock.Object);
        CreateEventRequest request = new CreateEventRequest
        {
            Titulo = "Palestra de C#",
            Descricao = "Evento acadêmico para testar validação.",
            DataInicio = DateTime.UtcNow.AddDays(2),
            DataFim = DateTime.UtcNow.AddDays(1),
            Local = "Sala 101"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(request, organizadorId: 1));
    }

    [Fact]
    public async Task UpdateAsync_UsuarioNaoOrganizador_LancaUnauthorizedException()
    {
        EventService service = new EventService(_eventRepositoryMock.Object);
        UpdateEventRequest request = new UpdateEventRequest
        {
            Titulo = "Palestra atualizada",
            Descricao = "Descrição válida para atualização.",
            DataInicio = DateTime.UtcNow.AddDays(2),
            DataFim = DateTime.UtcNow.AddDays(3),
            Local = "Auditório"
        };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(new Event { Id = 10, OrganizadorId = 99 });

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => service.UpdateAsync(id: 10, request, usuarioId: 1));
    }

    [Fact]
    public async Task GetAllAsync_StatusInvalido_LancaInvalidOperationException()
    {
        EventService service = new EventService(_eventRepositoryMock.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.GetAllAsync("Arquivado", organizadorId: null));
    }

    [Fact]
    public async Task GetAllAsync_OrganizadorInvalido_LancaInvalidOperationException()
    {
        EventService service = new EventService(_eventRepositoryMock.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.GetAllAsync(status: null, organizadorId: 0));
    }
}
