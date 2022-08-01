using Academia.JwtWrapper;
using Microsoft.AspNetCore.Mvc;

namespace Novit.Academia.ChallengePlaylist.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly JwtService jwtService;
    public AccountController(JwtService jwtService)
    {
        this.jwtService = jwtService;
    }
    
    [HttpPost]
    public ActionResult Login([FromBody] UsuarioAplicacion usuario)
    {
        var token = jwtService.CrearToken(usuario);
        if (string.IsNullOrEmpty(token))
            return Unauthorized("NO AUTORIZADO");
        return Ok(token);
    }
}