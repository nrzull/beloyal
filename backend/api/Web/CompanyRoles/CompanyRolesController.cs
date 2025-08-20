using Beloyal.API.Companies;
using Beloyal.API.Web.CompanyRoles.GetRoles;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.CompanyRoles;

[ApiController]
[Route("/api/company/company-roles")]
[WithAuth]
[WithCompanyAccess]
public class CompanyRolesController : ControllerBase
{
  public async Task<IActionResult> GetRoles(
    [FromServices] GetRolesEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetRolesResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
