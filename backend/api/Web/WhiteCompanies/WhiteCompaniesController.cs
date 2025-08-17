using Beloyal.API.Admin;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.WhiteCompanies.AddCompany;
using Beloyal.API.Web.WhiteCompanies.GetWhiteCompanies;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.WhiteCompanies;

[ApiController]
[Route("/api/white-companies")]
[WithAuth]
[WithUser]
public class WhiteCompaniesController : ControllerBase
{
  [HttpGet]
  [WithAdminAccess(nameof(AdminPermissions.white_companies_read))]
  public async Task<IActionResult> GetWhiteCompanies(
    [FromServices] GetWhiteCompaniesEndpoint endpoint
  ) =>
    await endpoint.Handle() switch
    {
      GetWhiteCompaniesResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpPost]
  [WithAdminAccess(nameof(AdminPermissions.white_companies_write))]
  public async Task<IActionResult> AddCompany(
    [FromServices] AddCompanyEndpoint endpoint,
    [FromBody] AddCompanyRequest req
  ) =>
    await endpoint.Handle(req) switch
    {
      AddCompanyResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
