using AcademicEvents.Application.DTOs.Event;
using AcademicEvents.Application.Services;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;
using Moq;

namespace AcademicEvents.Tests;

/// <summary>
/// Unit tests for the main EventService business rules.
/// </summary>
public class EventServiceTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();

    [Fact]
    public async Task CreateAsync_WhenEndDateIsBeforeStartDate_ThrowsInvalidOperationException()
    {
        EventService service = new EventService(_eventRepositoryMock.Object);
        CreateEventRequest request = new CreateEventRequest
        {
            Title = "Palestra de C#",
            Description = "Evento acadêmico para testar validação.",
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(1),
            Location = "Sala 101"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(request, organizerId: 1));
    }

    [Fact]
    public async Task CreateAsync_WhenTextsHaveSpaces_SavesTrimmedTexts()
    {
        EventService service = new EventService(_eventRepositoryMock.Object);
        CreateEventRequest request = new CreateEventRequest
        {
            Title = "  Palestra de C#  ",
            Description = "  Descrição válida para o evento acadêmico.  ",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            Location = "  Auditório  "
        };

        _eventRepositoryMock
            .Setup(repository => repository.CreateAsync(It.Is<Event>(evento =>
                evento.Title == "Palestra de C#"
                && evento.Description == "Descrição válida para o evento acadêmico."
                && evento.Location == "Auditório")))
            .ReturnsAsync((Event evento) =>
            {
                evento.Id = 1;
                return evento;
            });

        EventResponse response = await service.CreateAsync(request, organizerId: 1);

        Assert.Equal("Palestra de C#", response.Title);
        Assert.Equal("Auditório", response.Location);
    }

    [Fact]
    public async Task UpdateAsync_WhenUserIsNotOrganizer_ThrowsUnauthorizedException()
    {
        EventService service = new EventService(_eventRepositoryMock.Object);
        UpdateEventRequest request = new UpdateEventRequest
        {
            Title = "Palestra atualizada",
            Description = "Descrição válida para atualização.",
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(3),
            Location = "Auditório"
        };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(new Event { Id = 10, OrganizerId = 99 });

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => service.UpdateAsync(id: 10, request, userId: 1));
    }

    [Fact]
    public async Task GetAllAsync_WhenStatusIsInvalid_ThrowsInvalidOperationException()
    {
        EventService service = new EventService(_eventRepositoryMock.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.GetAllAsync("Arquivado", organizerId: null));
    }

    [Fact]
    public async Task GetAllAsync_WhenNumericStatusIsOutOfEnum_ThrowsInvalidOperationException()
    {
        EventService service = new EventService(_eventRepositoryMock.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.GetAllAsync("99", organizerId: null));
    }

    [Fact]
    public async Task GetAllAsync_WhenOrganizerIdIsInvalid_ThrowsInvalidOperationException()
    {
        EventService service = new EventService(_eventRepositoryMock.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.GetAllAsync(status: null, organizerId: 0));
    }

    [Fact]
    public async Task UpdateAsync_WhenStatusIsOutOfEnum_ThrowsInvalidOperationException()
    {
        EventService service = new EventService(_eventRepositoryMock.Object);
        UpdateEventRequest request = new UpdateEventRequest
        {
            Title = "Palestra atualizada",
            Description = "Descrição válida para atualização.",
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(3),
            Location = "Auditório",
            EventStatus = (AcademicEvents.Domain.Enums.EventStatus)99
        };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(new Event { Id = 10, OrganizerId = 1 });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.UpdateAsync(id: 10, request, userId: 1));
    }
}
