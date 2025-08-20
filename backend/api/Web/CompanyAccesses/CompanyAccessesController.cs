using Beloyal.API.Companies;
using Beloyal.API.Web.CompanyAccesses.GetAccesses;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.CompanyAccesses;

[ApiController]
[Route("/api/company/company-accesses")]
[WithAuth]
[WithCompanyAccess]
public class CompanyAccessesController : ControllerBase
{
  public async Task<IActionResult> GetCompanyAccesses(
    [FromServices] GetAccessesEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetAccessesResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
