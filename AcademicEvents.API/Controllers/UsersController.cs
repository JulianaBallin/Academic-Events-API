using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller de dados do usuario autenticado.
/// </summary>
[ApiController]
[Route("api")]
public class UsersController : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        // as claims do JWT ficam disponiveis no User do ControllerBase
        string id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        string email = User.FindFirst(ClaimTypes.Email)!.Value;
        string nome = User.FindFirst(ClaimTypes.Name)!.Value;

        return Ok(new { Id = id, Email = email, Nome = nome });
    }
}
