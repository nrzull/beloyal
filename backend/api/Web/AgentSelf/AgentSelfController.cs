using Beloyal.API.Auth;
using Beloyal.API.Users;
using Beloyal.API.Web.AgentSelf.GetSelf;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.AgentSelf;

[ApiController]
[Route("/api/agent/self")]
[WithAuth]
[WithUser]
public class AgentSelfController(IHostEnvironment env) : ControllerBase
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
}
