using Beloyal.API.Auth;
using Beloyal.API.Web.Auth.Confirm;
using Beloyal.API.Web.Auth.GetLoginVariants;
using Beloyal.API.Web.Auth.Login;
using Beloyal.API.Web.Auth.LoginAsTgCustomer;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Auth;

[ApiController]
[Route("/api/auth")]
public class AuthController(IHostEnvironment env) : ControllerBase
{
  public record LoginEndpointResponse(DateTimeOffset ExpiredAt);

  [HttpGet]
  public Results<Ok<GetLoginVariantsResponse.Ok>, BadRequest> GetLoginVariants(
    [FromServices] GetLoginVariantsEndpoint endpoint
  ) =>
    endpoint.Handle() switch
    {
      GetLoginVariantsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost]
  public async Task<Results<Ok<LoginEndpointResponse>, BadRequest, NotFound>> Login(
    [FromBody] LoginRequest req,
    [FromServices] LoginEndpoint endpoint
  ) =>
    await endpoint.Handle(new(req.Phone)) switch
    {
      LoginResponse.UserNotFound => TypedResults.NotFound(),
      LoginResponse.Ok ok => TypedResults.Ok(new LoginEndpointResponse(ExpiredAt: ok.ExpiredAt)),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost("confirm")]
  [WithAccessToken]
  public async Task<Results<Ok, BadRequest>> Confirm(
    [FromBody] ConfirmRequest req,
    [FromServices] ConfirmEndpoint endpoint,
    [FromServices] AccessToken? accessToken
  ) =>
    await endpoint.Handle(new(req.Phone), req.Code, accessToken) switch
    {
      ConfirmResponse.Ok ok => new Func<Ok>(() =>
      {
        HttpContext.SetAuthCookies(ok.AccessToken, ok.RefreshToken, env.IsProduction());
        return TypedResults.Ok();
      })(),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost("customer/telegram")]
  public async Task<
    Results<Ok<LoginAsTgCustomerResponse.Ok>, BadRequest<string>, BadRequest>
  > LoginAsCustomer(
    [FromServices] LoginAsTgCustomerEndpoint endpoint,
    [FromBody] LoginAsTgCustomerRequest req
  ) =>
    await endpoint.Handle(req) switch
    {
      LoginAsTgCustomerResponse.Ok ok => TypedResults.Ok(ok),
      LoginAsTgCustomerResponse.InvalidUser => TypedResults.BadRequest("invalid user"),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("logout")]
  public IActionResult Logout()
  {
    HttpContext.Response.Cookies.Delete(
      AuthContext.GetAccessTokenName(),
      new()
      {
        Path = "/",
        SameSite = env.IsProduction() ? SameSiteMode.None : SameSiteMode.Unspecified,
        HttpOnly = true,
        Secure = env.IsProduction(),
        Domain = HttpContext.Request.Host.Host,
      }
    );
    return Ok();
  }
}
