using AcademicEvents.Application.DTOs.Activity;
using AcademicEvents.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AcademicEvents.Exceptions;
using System.Security.Claims;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Endpoints for managing academic event activities.
/// </summary>
[ApiController]
[Route("api/activity")]
[Produces("application/json")]
public class ActivityController : ControllerBase
{
    private readonly IActivityService _service;

    public ActivityController(IActivityService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns an activity by id.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        ActivityResponse? activity = await _service.GetByIdAsync(id);
        return Ok(activity);
    }

    /// <summary>
    /// Creates a new activity. Only the event organizer can create activities for the event.
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(CreateActivityRequest request)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        ActivityResponse activity = await _service.CreateAsync(request, userId);
        return CreatedAtAction(nameof(GetById), new { id = activity.Id }, activity);
    }

    /// <summary>
    /// Returns all activities for an event by EventId.
    /// </summary>
    [HttpGet("event/{eventId:int}")]
    [ProducesResponseType(typeof(List<ActivityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByEventId(int eventId)
    {
        List<ActivityResponse> activities = await _service.GetByEventIdAsync(eventId);
        return Ok(activities);
    }

    /// <summary>
    /// Updates an activity. Only the event organizer can edit activities.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(int id, UpdateActivityRequest request)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        ActivityResponse response = await _service.UpdateAsync(id, request, userId);
        return Ok(response);
    }

    /// <summary>
    /// Deletes an activity. Only the event organizer can remove activities.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _service.DeleteAsync(id, userId);
        return NoContent();
    }
}
