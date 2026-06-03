using AcademicEvents.Application.DTOs.Reaction;
using AcademicEvents.Application.Services;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Enums;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;
using Moq;

namespace AcademicEvents.Tests;

/// <summary>
/// Unit tests for event reaction business rules./// </summary>
public class ReactionServiceTests
{
    private readonly Mock<IReactionRepository> _reactionRepositoryMock = new();
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();

    [Fact]
    public async Task CreateAsync_WhenEventDoesNotExist_ThrowsNotFoundException()
    {
        ReactionService service = new ReactionService(
            _reactionRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateReactionRequest request = new CreateReactionRequest
        {
            EventId = 404,
            Type = ReactionType.Like
        };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(404))
            .ReturnsAsync((Event?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.CreateAsync(request, userId: 1));
    }

    [Fact]
    public async Task GetByEventAsync_WhenIdIsInvalid_ThrowsInvalidOperationException()
    {
        ReactionService service = new ReactionService(
            _reactionRepositoryMock.Object,
            _eventRepositoryMock.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.GetByEventAsync(eventId: 0));
    }

    [Fact]
    public async Task CreateAsync_WhenReactionAlreadyExists_ThrowsInvalidOperationException()
    {
        ReactionService service = new ReactionService(
            _reactionRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateReactionRequest request = new CreateReactionRequest
        {
            EventId = 7,
            Type = ReactionType.WillParticipate
        };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(7))
            .ReturnsAsync(new Event { Id = 7, OrganizerId = 2 });

        _reactionRepositoryMock
            .Setup(repository => repository.GetByUserEventAsync(1, 7))
            .ReturnsAsync(new Reaction { Id = 3, UserId = 1, EventId = 7 });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(request, userId: 1));
    }

    [Fact]
    public async Task CreateAsync_WhenReactionTypeIsOutOfEnum_ThrowsInvalidOperationException()
    {
        ReactionService service = new ReactionService(
            _reactionRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateReactionRequest request = new CreateReactionRequest
        {
            EventId = 7,
            Type = (ReactionType)99
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(request, userId: 1));
    }
}
