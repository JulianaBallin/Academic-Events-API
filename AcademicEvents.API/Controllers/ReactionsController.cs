using System.Security.Claims;
using AcademicEvents.Application.DTOs.Reaction;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller de reações a eventos.
/// </summary>
[ApiController]
[Route("api/reactions")]
public class ReactionsController : ControllerBase
{
    private readonly IReactionService _service;

    public ReactionsController(IReactionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetByEvento([FromQuery] int eventoId)
    {
        return Ok(await _service.GetByEventoAsync(eventoId));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateReactionRequest request)
    {
        try
        {
            int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            ReactionResponse response = await _service.CreateAsync(request, usuarioId);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize]
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
            return Forbid(ex.Message);
        }
    }
}
