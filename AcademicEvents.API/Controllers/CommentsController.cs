using System.Security.Claims;
using AcademicEvents.Application.DTOs.Comment;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller de comentários em eventos.
/// Leitura pública, escrita e remoção exigem autenticação.
/// </summary>
[ApiController]
[Route("api/comments")]
[Produces("application/json")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _service;

    public CommentsController(ICommentService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista todos os comentários de um evento.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CommentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEvento([FromQuery] int eventoId)
    {
        return Ok(await _service.GetByEventoAsync(eventoId));
    }

    /// <summary>
    /// Adiciona um comentário a um evento.
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(CreateCommentRequest request)
    {
        int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        CommentResponse response = await _service.CreateAsync(request, usuarioId);
        return Ok(response);
    }

    /// <summary>
    /// Remove um comentário. Só o autor pode deletar.
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
