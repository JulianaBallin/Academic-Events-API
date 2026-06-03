using AcademicEvents.Application.DTOs.Activity;
using AcademicEvents.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AcademicEvents.Exceptions;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller de eventos acadêmicos.
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
    /// Retorna uma atividade pelo id;
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        ActivityResponse? activity = await _service.GetByIdAsync(id);
        return Ok(activity);
    }

    /// <summary>
    /// Cria um novo evento. O organizador é o usuário autenticado.
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateActivityRequest request)
    {
        ActivityResponse activity = await _service.CreateAsync(request);

        return CreatedAtAction(nameof(GetById),new { id = activity.Id }, activity);
    }


    /// <summary>
    /// Retorna todas as atividade de um evento pelo EventId;
    /// </summary>
    [HttpGet("event/{eventId:int}")]
    [ProducesResponseType(typeof(List<ActivityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByEventId(int eventId)
    {
        List<ActivityResponse> activities =
            await _service.GetByEventIdAsync(eventId);

        return Ok(activities);
    }
}