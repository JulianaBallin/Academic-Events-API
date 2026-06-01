using AcademicEvents.Application.DTOs.Registration;
using AcademicEvents.Application.Services;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;
using Moq;

namespace AcademicEvents.Tests;

/// <summary>
/// Testes unitários das regras de inscrição em eventos.
/// </summary>
public class RegistrationServiceTests
{
    private readonly Mock<IRegistrationRepository> _registrationRepositoryMock = new();
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();

    [Fact]
    public async Task CreateAsync_EventoNaoExiste_LancaNotFoundException()
    {
        RegistrationService service = new RegistrationService(
            _registrationRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateRegistrationRequest request = new CreateRegistrationRequest { EventoId = 999 };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(999))
            .ReturnsAsync((Event?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.CreateAsync(request, usuarioId: 1));
    }

    [Fact]
    public async Task CreateAsync_InscricaoDuplicada_LancaInscricaoDuplicadaException()
    {
        RegistrationService service = new RegistrationService(
            _registrationRepositoryMock.Object,
            _eventRepositoryMock.Object);

        CreateRegistrationRequest request = new CreateRegistrationRequest { EventoId = 5 };

        _eventRepositoryMock
            .Setup(repository => repository.GetByIdAsync(5))
            .ReturnsAsync(new Event { Id = 5, OrganizadorId = 2 });

        _registrationRepositoryMock
            .Setup(repository => repository.GetByUsuarioEEventoAsync(1, 5))
            .ReturnsAsync(new Registration { Id = 20, UsuarioId = 1, EventoId = 5 });

        await Assert.ThrowsAsync<InscricaoDuplicadaException>(
            () => service.CreateAsync(request, usuarioId: 1));
    }
}
