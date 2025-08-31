using Beloyal.API.Companies;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Mailings.CreateMailing;
using Beloyal.API.Web.Mailings.GetMailings;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Mailings;

[ApiController]
[Route("/api/company/mailings")]
[WithAuth]
[WithCompanyAccess]
public class MailingsController : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetMailings(
    [FromServices] GetMailingsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetMailingsResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpPost]
  public async Task<IActionResult> CreateMailing(
    [FromServices] CreateMailingEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateMailingRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateMailingResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
