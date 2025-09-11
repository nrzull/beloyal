using Beloyal.API.Auth;
using Beloyal.API.Users;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Self.CreateCompany;
using Beloyal.API.Web.Self.GetSelf;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Self;

[ApiController]
[WithAuth]
[WithUser]
[Route("/api/self")]
public class SelfController(IHostEnvironment env) : ControllerBase
{
  [HttpGet]
  [WithAccessToken]
  public async Task<Results<Ok<GetSelfResponse.Ok>, BadRequest>> GetSelf(
    [FromServices] GetSelfEndpoint endpoint,
    [FromServices] User? user,
    [FromServices] AccessToken? accessToken
  ) =>
    await endpoint.Handle(user.Ensure(), accessToken!) switch
    {
      (null, GetSelfResponse.Ok ok) => TypedResults.Ok(ok),
      (EncodedAccessToken newToken, GetSelfResponse.Ok ok) => new Func<Ok<GetSelfResponse.Ok>>(() =>
      {
        HttpContext.SetAccessTokenCookie(newToken, env.IsProduction());
        return TypedResults.Ok(ok);
      })(),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost("company")]
  public async Task<Results<Ok<CreateCompanyResponse.Ok>, BadRequest>> CreateCompany(
    [FromServices] CreateCompanyEndpoint endpoint,
    [FromServices] User? user,
    [FromBody] CreateCompanyRequest req
  ) =>
    await endpoint.Handle(user.Ensure(), req) switch
    {
      CreateCompanyResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
