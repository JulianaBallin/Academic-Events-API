using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller for authenticated user data.
/// Reads user information directly from the JWT token claims.
/// </summary>
[ApiController]
[Route("api")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    /// <summary>
    /// Returns the current user's data extracted from the JWT token.
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Me()
    {
        // JWT claims are available through the User property from ControllerBase.
        string id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        string email = User.FindFirst(ClaimTypes.Email)!.Value;
        string name = User.FindFirst(ClaimTypes.Name)!.Value;

        return Ok(new { Id = id, Email = email, Name = name });
    }
}
