using System.Security.Claims;
using AcademicEvents.Application.DTOs.Comment;
using AcademicEvents.Application.Interfaces;
using AcademicEvents.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller for event comments.
/// Reading is public, while creating and deleting comments require authentication.
/// </summary>
[ApiController]
[Route("api/comments")]
[Produces("application/json")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    /// Lists all comments for an event.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByEvent([FromQuery] int eventId)
    {
        if (eventId <= 0)
            throw new InvalidOperationException("The event id must be greater than zero.");

        return Ok(await _commentService.GetByEventAsync(eventId));
    }

    /// <summary>
    /// Adds a comment to an event.
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(CreateCommentRequest request)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        CommentResponse response = await _commentService.CreateAsync(request, userId);

        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Deletes a comment. Only the author can delete it.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _commentService.DeleteAsync(id, userId);

        return NoContent();
    }
}
