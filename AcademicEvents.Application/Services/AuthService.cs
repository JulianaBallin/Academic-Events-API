using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AcademicEvents.Application.DTOs.Auth;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Domain.Entities;
using AcademicEvents.Domain.Interfaces;
using AcademicEvents.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AcademicEvents.Application.Services;

/// <summary>
/// Provides authentication services, including password hashing with BCrypt
/// and JWT bearer token generation.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Normalize email before saving to avoid duplicates like "Test@" and "test@".
        string normalizedEmail = (request.Email ?? string.Empty).Trim().ToLowerInvariant();
        string normalizedName = (request.Name ?? string.Empty).Trim();
        string password = request.Password ?? string.Empty;

        if (string.IsNullOrWhiteSpace(normalizedName))
            throw new InvalidOperationException("Name is required.");

        if (string.IsNullOrWhiteSpace(normalizedEmail))
            throw new InvalidOperationException("Email is required.");

        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Password is required.");

        if (await _repository.GetByEmailAsync(normalizedEmail) is not null)
            throw new DuplicateEmailException("A user with this email address already exists.");

        // BCrypt generates and stores the salt automatically.
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        User user = new User
        {
            Name = normalizedName,
            Email = normalizedEmail,
            Password = passwordHash
        };

        User createdUser = await _repository.CreateAsync(user);
        return GenerateTokenResponse(createdUser);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // normalizes email before search
        string normalizedEmail = (request.Email ?? string.Empty).Trim().ToLowerInvariant();
        string password = request.Password ?? string.Empty;
        User? user = await _repository.GetByEmailAsync(normalizedEmail);

        // returns the same error despite invalid email or password (security)
        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            throw new InvalidCredentialsException("Invalid email or password.");

        return GenerateTokenResponse(user);
    }

    private AuthResponse GenerateTokenResponse(User user)
    {
        // get app_settings configs to assign token
        string key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key not configured.");
        string issuer = _configuration["Jwt:Issuer"]!;
        string audience = _configuration["Jwt:Audience"]!;
        int expiresIn = int.Parse(_configuration["Jwt:ExpiresInHours"] ?? "8");

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        DateTime expires = DateTime.UtcNow.AddHours(expiresIn);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Name = user.Name,
            Email = user.Email,
            ExpiresIn = expires
        };
    }
}
