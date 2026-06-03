using AcademicEvents.Application.DTOs.Auth;
using AcademicEvents.Application.Services;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace AcademicEvents.Tests;

/// <summary>
/// Unit tests for authentication business rules.
/// </summary>
public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsDuplicateEmailException()
    {
        AuthService service = new AuthService(
            _userRepositoryMock.Object,
            _configurationMock.Object);

        RegisterRequest request = new RegisterRequest
        {
            Name = "Maria Silva",
            Email = "maria@teste.com",
            Password = "Senha123"
        };

        _userRepositoryMock
            .Setup(repository => repository.GetByEmailAsync("maria@teste.com"))
            .ReturnsAsync(new User { Id = 1, Email = "maria@teste.com" });

        await Assert.ThrowsAsync<DuplicateEmailException>(
            () => service.RegisterAsync(request));
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailAndNameHaveSpaces_NormalizesBeforeSaving()
    {
        AuthService service = new AuthService(
            _userRepositoryMock.Object,
            _configurationMock.Object);

        RegisterRequest request = new RegisterRequest
        {
            Name = "  Maria Silva  ",
            Email = "  MARIA@TESTE.COM  ",
            Password = "Senha123"
        };

        _configurationMock.Setup(configuration => configuration["Jwt:Key"])
            .Returns("chave-secreta-de-teste-com-mais-de-32-caracteres");
        _configurationMock.Setup(configuration => configuration["Jwt:Issuer"])
            .Returns("AcademicEventsAPI");
        _configurationMock.Setup(configuration => configuration["Jwt:Audience"])
            .Returns("AcademicEventsClientes");
        _configurationMock.Setup(configuration => configuration["Jwt:ExpiresInHours"])
            .Returns("8");

        _userRepositoryMock
            .Setup(repository => repository.GetByEmailAsync("maria@teste.com"))
            .ReturnsAsync((User?)null);

        _userRepositoryMock
            .Setup(repository => repository.CreateAsync(It.Is<User>(user =>
                user.Name == "Maria Silva"
                && user.Email == "maria@teste.com"
                && !string.IsNullOrWhiteSpace(user.Password))))
            .ReturnsAsync((User user) =>
            {
                user.Id = 10;
                return user;
            });

        AuthResponse response = await service.RegisterAsync(request);

        Assert.Equal("Maria Silva", response.Name);
        Assert.Equal("maria@teste.com", response.Email);
        Assert.False(string.IsNullOrWhiteSpace(response.Token));
    }

    [Fact]
    public async Task LoginAsync_WhenEmailDoesNotExist_ThrowsInvalidCredentialsException()
    {
        AuthService service = new AuthService(
            _userRepositoryMock.Object,
            _configurationMock.Object);

        LoginRequest request = new LoginRequest
        {
            Email = "inexistente@teste.com",
            Password = "Senha123"
        };

        _userRepositoryMock
            .Setup(repository => repository.GetByEmailAsync("inexistente@teste.com"))
            .ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<InvalidCredentialsException>(
            () => service.LoginAsync(request));
    }
}
