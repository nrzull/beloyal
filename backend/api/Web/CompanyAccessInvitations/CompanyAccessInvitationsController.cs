using Beloyal.API.Companies;
using Beloyal.API.Web.CompanyAccessInvitations.CreateInvitation;
using Beloyal.API.Web.CompanyAccessInvitations.GetInvitations;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.CompanyAccessInvitations;

[ApiController]
[Route("/api/company/company-access-invitations")]
[WithAuth]
[WithCompanyAccess]
public class CompanyAccessInvitationsController : ControllerBase
{
  [HttpGet]
  public async Task<Results<Ok<GetInvitationsResponse.Ok>, BadRequest>> GetInvitations(
    [FromServices] GetInvitationsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetInvitationsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost]
  public async Task<Results<Ok<CreateInvitationResponse.Ok>, BadRequest>> CreateInvitation(
    [FromServices] CreateInvitationEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateInvitationRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateInvitationResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
