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
/// Implementacao do service de autenticacao.
/// Usa BCrypt para hash de senha e gera JWT Bearer Token.
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
        // nao pode ter dois usuarios com o mesmo email
        if (await _repository.GetByEmailAsync(request.Email) is not null)
            throw new DuplicateEmailException("Esse email ja esta em uso.");

        // BCrypt cuida do salt automaticamente, nao precisa passar nada extra
        string senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

        User usuario = new User
        {
            Nome = request.Nome,
            Email = request.Email,
            SenhaHash = senhaHash
        };

        User criado = await _repository.CreateAsync(usuario);
        return GerarTokenResponse(criado);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        User? usuario = await _repository.GetByEmailAsync(request.Email);

        // retorna o mesmo erro independente se email ou senha estao errados (seguranca)
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
            throw new InvalidCredentialsException("Email ou senha invalidos.");

        return GerarTokenResponse(usuario);
    }

    private AuthResponse GerarTokenResponse(User usuario)
    {
        // pega as configs do appsettings pra assinar o token
        string key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key nao configurada.");
        string issuer = _configuration["Jwt:Issuer"]!;
        string audience = _configuration["Jwt:Audience"]!;
        int expiresIn = int.Parse(_configuration["Jwt:ExpiresInHours"] ?? "8");

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome)
        };

        DateTime expira = DateTime.UtcNow.AddHours(expiresIn);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expira,
            signingCredentials: credentials
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Nome = usuario.Nome,
            Email = usuario.Email,
            ExpiraEm = expira
        };
    }
}
