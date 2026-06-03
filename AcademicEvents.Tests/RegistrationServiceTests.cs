using AcademicEvents.Application.DTOs.Registration;
using AcademicEvents.Application.Services;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;
using Moq;

namespace AcademicEvents.Tests;

/// <summary>
/// Unit tests for event registration business rules.
/// </summary>
public class RegistrationServiceTests
{
    private readonly Mock<IRegistrationRepository> _registrationRepositoryMock = new();
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();

    [Fact]
    public async Task CreateAsync_WhenEventDoesNotExist_ThrowsNotFoundException()
    {
        RegistrationService service = new RegistrationService(
            _registrationRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateRegistrationRequest request = new CreateRegistrationRequest { EventId = 999 };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(999))
            .ReturnsAsync((Event?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.CreateAsync(request, userId: 1));
    }

    [Fact]
    public async Task CreateAsync_WhenEventIdIsInvalid_ThrowsInvalidOperationException()
    {
        RegistrationService service = new RegistrationService(
            _registrationRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateRegistrationRequest request = new CreateRegistrationRequest { EventId = 0 };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(request, userId: 1));
    }

    [Fact]
    public async Task CreateAsync_WhenRegistrationAlreadyExists_ThrowsDuplicateRegistrationException()
    {
        RegistrationService service = new RegistrationService(
            _registrationRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateRegistrationRequest request = new CreateRegistrationRequest { EventId = 5 };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(5))
            .ReturnsAsync(new Event { Id = 5, OrganizerId = 2 });

        _registrationRepositoryMock
            .Setup(repository => repository.GetByUserEventAsync(1, 5))
            .ReturnsAsync(new Registration { Id = 20, UserId = 1, EventId = 5 });

        await Assert.ThrowsAsync<DuplicateRegistrationException>(
            () => service.CreateAsync(request, userId: 1));
    }
}
