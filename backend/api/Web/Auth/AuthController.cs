using Beloyal.API.Auth;
using Beloyal.API.Web.Auth.Confirm;
using Beloyal.API.Web.Auth.Login;
using Beloyal.API.Web.Auth.LoginAsTgCustomer;
using Beloyal.API.Web.Filters;
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
  ) =>
    await endpoint.Handle(new(req.Phone)) switch
    {
      LoginResponse.Ok ok => Ok(new { ok.ExpiredAt, Code = env.IsDevelopment() ? ok.Code : null }),
      _ => BadRequest(),
    };

  [HttpPost("confirm")]
  [WithAccessToken]
  public async Task<IActionResult> Confirm(
    [FromBody] ConfirmRequest req,
    [FromServices] ConfirmEndpoint endpoint,
    [FromServices] AccessToken? accessToken
  ) =>
    await endpoint.Handle(new(req.Phone), req.Code, accessToken) switch
    {
      ConfirmResponse.Ok ok => new Func<IActionResult>(() =>
      {
        HttpContext.SetAccessTokenCookie(ok.Token, env.IsProduction());
        return Ok();
      })(),
      _ => BadRequest(),
    };

  [HttpPost("customer/telegram")]
  public async Task<IActionResult> LoginAsCustomer(
    [FromServices] LoginAsTgCustomerEndpoint endpoint,
    [FromBody] LoginAsTgCustomerRequest req
  ) =>
    await endpoint.Handle(req) switch
    {
      LoginAsTgCustomerResponse.Ok ok => Ok(ok),
      LoginAsTgCustomerResponse.InvalidUser => BadRequest("invalid user"),
      _ => BadRequest(),
    };

  [HttpGet("logout")]
  public IActionResult Logout()
  {
    HttpContext.Response.Cookies.Delete(AuthContext.GetAccessTokenName());
    return Ok();
  }
}
