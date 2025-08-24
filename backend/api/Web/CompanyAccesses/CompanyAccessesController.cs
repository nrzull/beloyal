using Beloyal.API.Companies;
using Beloyal.API.Web.CompanyAccesses.GetAccess;
using Beloyal.API.Web.CompanyAccesses.GetAccesses;
using Beloyal.API.Web.CompanyAccesses.UpdateAccess;
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

  [HttpGet("{id}")]
  public async Task<IActionResult> GetAccess(
    [FromServices] GetAccessEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid companyAccessId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), CompanyAccessId.From(companyAccessId)) switch
    {
      GetAccessResponse.NotFound => NotFound(),
      GetAccessResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpPatch("{id}")]
  public async Task<IActionResult> UpdateCompanyAccess(
    [FromServices] UpdateAccessEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] UpdateAccessRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      UpdateAccessResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
