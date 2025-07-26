using Beloyal.API.Web.Auth.Login;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Auth;

[ApiController]
[Route("/api/auth")]
public class AuthController(IHostEnvironment env) : ControllerBase
{
  [HttpPost]
  public async Task<IActionResult> Login(
    [FromBody] LoginRequest req,
    [FromServices] LoginEndpoint endpoint
  )
  {
    return await endpoint.Handle(new(req.Email)) switch
    {
      LoginResponse.Ok ok => Ok(new { ok.ExpiredAt, Code = env.IsDevelopment() ? ok.Code : null }),
      _ => BadRequest(),
    };
  }
}
