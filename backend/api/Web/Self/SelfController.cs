using Beloyal.API.Auth;
using Beloyal.API.Users;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Self.CreateCompany;
using Beloyal.API.Web.Self.GetCompanyAccessInvitations;
using Beloyal.API.Web.Self.GetSelf;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Self;

[ApiController]
[WithAuth]
[WithUser]
[Route("/api/self")]
public class SelfController : ControllerBase
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
      GetSelfResponse.Ok ok => Ok(ok),
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
