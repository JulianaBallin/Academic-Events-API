using System.Security.Claims;
using AcademicEvents.Application.DTOs.Event;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller for academic events.
/// GET routes are public. POST, PUT, and DELETE require authentication.
/// </summary>
[ApiController]
[Route("api/events")]
[Produces("application/json")]
public class EventsController : ControllerBase
{
    private readonly IEventService _evetService;

    public EventsController(IEventService evetService)
    {
        _evetService = evetService;
    }

    /// <summary>
    /// Lists all events. Filter by status and organizer using query string parameters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<EventResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] int? organizerId)
    {
        return Ok(await _evetService.GetAllAsync(status, organizerId));
    }

    /// <summary>
    /// Returns the events created by the authenticated user.
    /// </summary>
    [HttpGet("mine")]
    [Authorize]
    [ProducesResponseType(typeof(List<EventResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCreatedEvents()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return Ok(await _evetService.GetByOrganizerAsync(userId));
    }

    /// <summary>
    /// Returns an event by id.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        EventResponse? response = await _evetService.GetByIdAsync(id);
        if (response is null)
            throw new NotFoundException("Event not found.");

        return Ok(response);
    }

    /// <summary>
    /// Creates a new event. The organizer is the authenticated user.
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(EventResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateEventRequest request)
    {
        // get user id from JWT token
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        EventResponse response = await _evetService.CreateAsync(request, userId);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Updates an event. Only the organizer can edit it.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(int id, UpdateEventRequest request)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        EventResponse? response = await _evetService.UpdateAsync(id, request, userId);
        if (response is null)
            throw new NotFoundException("Event not found.");

        return Ok(response);
    }

    /// <summary>
    /// Deletes an event. Only the organizer can delete it.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _evetService.DeleteAsync(id, userId);
        return NoContent();
    }
}
