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
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByEvento([FromQuery] int eventoId)
    {
        if (eventoId <= 0)
            throw new InvalidOperationException("O id do evento deve ser maior que zero.");

        return Ok(await _service.GetByEventoAsync(eventoId));
    }

    /// <summary>
    /// Adiciona uma reação a um evento. Só uma por usuário.
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ReactionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(CreateReactionRequest request)
    {
        int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        ReactionResponse response = await _service.CreateAsync(request, usuarioId);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Remove uma reação. Só o autor pode deletar.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _service.DeleteAsync(id, usuarioId);
        return NoContent();
    }
}
