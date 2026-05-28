using AcademicEvents.Application.DTOs.Auth;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller de autenticacao. Todos os endpoints aqui sao publicos.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            return Ok(await _service.RegisterAsync(request));
        }
        catch (DuplicateEmailException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            return Ok(await _service.LoginAsync(request));
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}
