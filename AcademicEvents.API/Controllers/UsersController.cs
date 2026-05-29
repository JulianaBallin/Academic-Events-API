using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicEvents.API.Controllers;

/// <summary>
/// Controller de dados do usuário autenticado.
/// Lê as informações diretamente das claims do token JWT.
/// </summary>
[ApiController]
[Route("api")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    /// <summary>
    /// Retorna os dados do usuário atual extraídos do token JWT.
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Me()
    {
        // as claims do JWT ficam disponíveis no User do ControllerBase
        string id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        string email = User.FindFirst(ClaimTypes.Email)!.Value;
        string nome = User.FindFirst(ClaimTypes.Name)!.Value;

        return Ok(new { Id = id, Email = email, Nome = nome });
    }
}
