using System.Security.Claims;
using AcademicEvents.Application.DTOs.Reaction;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller for event reactions.
/// Each user can have at most one reaction per event.
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
    /// Lists all reactions for an event.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ReactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByEvent([FromQuery] int eventId)
    {
        if (eventId <= 0)
            throw new InvalidOperationException("The event id must be greater than zero.");

        return Ok(await _service.GetByEventAsync(eventId));
    }

    /// <summary>
    /// Adds a reaction to an event. Each user can react only once per event.
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ReactionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(CreateReactionRequest request)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        ReactionResponse response = await _service.CreateAsync(request, userId);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Deletes a reaction. Only the author can delete it.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _service.DeleteAsync(id, userId);
        return NoContent();
    }
}
