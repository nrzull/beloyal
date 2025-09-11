using Beloyal.API.Auth;
using Beloyal.API.Users;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Self.AcceptCompanyAccessInvitation;
using Beloyal.API.Web.Self.CreateCompany;
using Beloyal.API.Web.Self.GetCompanyAccessInvitations;
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

  [HttpGet("company-access-invitations")]
  public async Task<
    Results<Ok<GetCompanyAccessInvitationsResponse.Ok>, BadRequest>
  > GetCompanyAccessInvitations(
    [FromServices] GetCompanyAccessInvitationsEndpoint endpoint,
    [FromServices] User? user
  ) =>
    await endpoint.Handle(user.Ensure()) switch
    {
      GetCompanyAccessInvitationsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost("company-access-invitations/{id}/accept")]
  public async Task<
    Results<Ok<AcceptCompanyAccessInvitationResponse.Ok>, BadRequest>
  > AcceptCompanyAccessInvitation(
    [FromServices] AcceptCompanyAccessInvitationEndpoint endpoint,
    [FromServices] User? user,
    [FromRoute(Name = "id")] Guid companyAccessInvitationId
  ) =>
    await endpoint.Handle(user.Ensure(), new(companyAccessInvitationId)) switch
    {
      AcceptCompanyAccessInvitationResponse.Ok ok => TypedResults.Ok(ok),
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
