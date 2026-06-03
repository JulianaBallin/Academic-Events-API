using AcademicEvents.Application.DTOs.Activity;
using AcademicEvents.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AcademicEvents.Exceptions;
using System.Security.Claims;

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
    /// Cria uma nova atividade. Só o organizador do evento pode criar tarefas associadas. 
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(CreateActivityRequest request)
    {
        int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        ActivityResponse activity = await _service.CreateAsync(request,usuarioId);

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


    /// <summary>
    /// Edita uma atividade. Só o organizador do evento pode editar atividades associadas. 
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(int id, UpdateActivityRequest request)
    {
        
        int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        ActivityResponse response =
            await _service.UpdateAsync(id, request,usuarioId);

        return Ok(response);
    }



}