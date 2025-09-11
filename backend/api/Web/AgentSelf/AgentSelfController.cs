using Beloyal.API.Auth;
using Beloyal.API.Users;
using Beloyal.API.Web.AgentSelf.GetSelf;
using Beloyal.API.Web.AgentSelf.SetCompanyAccess;
using Beloyal.API.Web.AgentSelf.SetShopAccess;
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
  [ProducesResponseType(typeof(GetSelfResponse.Ok), StatusCodes.Status200OK)]
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

  [HttpPatch("company-access")]
  public async Task<Results<Ok, BadRequest>> SetCompanyAccess(
    [FromServices] SetCompanyAccessEndpoint endpoint,
    [FromServices] User? user,
    [FromBody] SetCompanyAccessRequest req
  ) =>
    await endpoint.Handle(user.Ensure(), req) switch
    {
      SetCompanyAccessResponse.Ok ok => new Func<Ok>(() =>
      {
        HttpContext.SetAccessTokenCookie(ok.Token, env.IsProduction());
        return TypedResults.Ok();
      })(),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("shop-access")]
  public async Task<Results<Ok, BadRequest>> SetShopAccess(
    [FromServices] SetShopAccessEndpoint endpoint,
    [FromServices] User? user,
    [FromBody] SetShopAccessRequest req
  ) =>
    await endpoint.Handle(user.Ensure(), req) switch
    {
      SetShopAccessResponse.Ok ok => new Func<Ok>(() =>
      {
        HttpContext.SetAccessTokenCookie(ok.Token, env.IsProduction());
        return TypedResults.Ok();
      })(),
      _ => TypedResults.BadRequest(),
    };
}
