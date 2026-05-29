using System.Security.Claims;
using AcademicEvents.Application.DTOs.Reaction;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller de reações a eventos.
/// Cada usuário pode ter no máximo uma reação por evento.
/// </summary>
[ApiController]
[Route("api/reactions")]
[Produces("application/json")]
public class ReactionsController : ControllerBase
{
    private readonly IReactionService _service;

    public ReactionsController(IReactionService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista todas as reações de um evento.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ReactionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEvento([FromQuery] int eventoId)
    {
        return Ok(await _service.GetByEventoAsync(eventoId));
    }

    /// <summary>
    /// Adiciona uma reação a um evento. Só uma por usuário.
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ReactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Remove uma reação. Só o autor pode deletar.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize]
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
