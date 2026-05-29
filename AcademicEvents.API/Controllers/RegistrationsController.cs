using System.Security.Claims;
using AcademicEvents.Application.DTOs.Registration;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller de inscrições em eventos.
/// Todos os endpoints são protegidos por autenticação JWT.
/// </summary>
[ApiController]
[Route("api/registrations")]
[Authorize]
[Produces("application/json")]
public class RegistrationsController : ControllerBase
{
    private readonly IRegistrationService _service;

    public RegistrationsController(IRegistrationService service)
    {
        _service = service;
    }

    /// <summary>
    /// Inscreve o usuário autenticado em um evento.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RegistrationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateRegistrationRequest request)
    {
        try
        {
            int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            RegistrationResponse response = await _service.CreateAsync(request, usuarioId);
            return Ok(response);
        }
        catch (InscricaoDuplicadaException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Lista as inscrições do usuário autenticado.
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(List<RegistrationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMinhas()
    {
        int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return Ok(await _service.GetByUsuarioAsync(usuarioId));
    }

    /// <summary>
    /// Cancela uma inscrição. Só o próprio usuário pode cancelar.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _service.DeleteAsync(id, usuarioId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
    }
}
