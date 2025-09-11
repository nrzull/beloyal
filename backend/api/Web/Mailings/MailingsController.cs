using Beloyal.API.Companies;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Mailings.CreateMailing;
using Beloyal.API.Web.Mailings.GetMailings;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Mailings;

[ApiController]
[Route("/api/company/mailings")]
[WithAuth]
[WithCompanyAccess]
public class MailingsController : ControllerBase
{
  [HttpGet]
  public async Task<Results<Ok<GetMailingsResponse.Ok>, BadRequest>> GetMailings(
    [FromServices] GetMailingsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetMailingsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost]
  public async Task<Results<Ok<CreateMailingResponse.Ok>, BadRequest>> CreateMailing(
    [FromServices] CreateMailingEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateMailingRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateMailingResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
