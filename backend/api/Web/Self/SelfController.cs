using Beloyal.API.Auth;
using Beloyal.API.Users;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Self.AcceptCompanyAccessInvitation;
using Beloyal.API.Web.Self.CreateCompany;
using Beloyal.API.Web.Self.GetCompanyAccessInvitations;
using Beloyal.API.Web.Self.GetSelf;
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
  public async Task<IActionResult> GetSelf(
    [FromServices] GetSelfEndpoint endpoint,
    [FromServices] User? user,
    [FromServices] AccessToken? accessToken
  ) =>
    await endpoint.Handle(user.Ensure(), accessToken!) switch
    {
      (null, GetSelfResponse.Ok ok) => Ok(ok),
      (EncodedAccessToken newToken, GetSelfResponse.Ok ok) => new Func<IActionResult>(() =>
      {
        HttpContext.SetAccessTokenCookie(newToken, env.IsProduction());
        return Ok(ok);
      })(),
      _ => BadRequest(),
    };

  [HttpGet("company-access-invitations")]
  public async Task<IActionResult> GetCompanyAccessInvitations(
    [FromServices] GetCompanyAccessInvitationsEndpoint endpoint,
    [FromServices] User? user
  ) =>
    await endpoint.Handle(user.Ensure()) switch
    {
      GetCompanyAccessInvitationsResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpPost("company-access-invitations/{id}/accept")]
  public async Task<IActionResult> AcceptCompanyAccessInvitation(
    [FromServices] AcceptCompanyAccessInvitationEndpoint endpoint,
    [FromServices] User? user,
    [FromRoute(Name = "id")] Guid companyAccessInvitationId
  ) =>
    await endpoint.Handle(user.Ensure(), new(companyAccessInvitationId)) switch
    {
      AcceptCompanyAccessInvitationResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpPost("company")]
  public async Task<IActionResult> CreateCompany(
    [FromServices] CreateCompanyEndpoint endpoint,
    [FromServices] User? user,
    [FromBody] CreateCompanyRequest req
  ) =>
    await endpoint.Handle(user.Ensure(), req) switch
    {
      CreateCompanyResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
