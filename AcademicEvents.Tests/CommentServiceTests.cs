using AcademicEvents.Application.DTOs.Comment;
using AcademicEvents.Application.Services;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;
using Moq;

namespace AcademicEvents.Tests;

/// <summary>
/// Unit tests for event comment rules.
/// </summary>
public class CommentServiceTests
{
    private readonly Mock<ICommentRepository> _commentRepositoryMock = new();
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();

    [Fact]
    public async Task CreateAsync_WhenEventDoesNotExist_ThrowsNotFoundException()
    {
        CommentService service = new CommentService(
            _commentRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateCommentRequest request = new CreateCommentRequest
        {
            EventId = 123,
            Content = "Comentário válido."
        };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(123))
            .ReturnsAsync((Event?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.CreateAsync(request, userId: 1));
    }

    [Fact]
    public async Task CreateAsync_WhenContentIsBlank_ThrowsInvalidOperationException()
    {
        CommentService service = new CommentService(
            _commentRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateCommentRequest request = new CreateCommentRequest
        {
            EventId = 1,
            Content = "   "
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(request, userId: 1));

        _eventRepositoryMock.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenContentHasSpaces_SavesTrimmedContent()
    {
        CommentService service = new CommentService(
            _commentRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateCommentRequest request = new CreateCommentRequest
        {
            EventId = 1,
            Content = "  Comentário válido.  "
        };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(new Event { Id = 1 });

        _commentRepositoryMock
            .Setup(repository => repository.CreateAsync(It.Is<Comment>(comment =>
                comment.Content == "Comentário válido.")))
            .ReturnsAsync((Comment comment) =>
            {
                comment.Id = 2;
                return comment;
            });

        CommentResponse response = await service.CreateAsync(request, userId: 1);

        Assert.Equal("Comentário válido.", response.Content);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserIsNotAuthor_ThrowsUnauthorizedException()
    {
        CommentService service = new CommentService(
            _commentRepositoryMock.Object,
            _eventRepositoryMock.Object);

        _commentRepositoryMock
            .Setup(repository => repository.GetByIdAsync(8))
            .ReturnsAsync(new Comment { Id = 8, UserId = 2, EventId = 1 });

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => service.DeleteAsync(id: 8, userId: 1));
    }
}
