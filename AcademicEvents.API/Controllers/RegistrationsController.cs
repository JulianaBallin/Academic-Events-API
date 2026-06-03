using System.Security.Claims;
using AcademicEvents.Application.DTOs.Registration;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller for event registrations.
/// All endpoints are protected by JWT authentication.
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
    /// Registers the authenticated user for an event.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RegistrationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(CreateRegistrationRequest request)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        RegistrationResponse response = await _service.CreateAsync(request, userId);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Lists the registrations of the authenticated user.
    /// </summary>
    [HttpGet("mine")]
    [ProducesResponseType(typeof(List<RegistrationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMine()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return Ok(await _service.GetByUserAsync(userId));
    }

    /// <summary>
    /// Cancels a registration. Only the registered user can cancel it.
    /// </summary>
    [HttpDelete("{id:int}")]
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
