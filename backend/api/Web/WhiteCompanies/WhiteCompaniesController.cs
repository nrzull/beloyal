using Beloyal.API.Admin;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.WhiteCompanies.AddCompany;
using Beloyal.API.Web.WhiteCompanies.GetWhiteCompanies;
using Microsoft.AspNetCore.Http.HttpResults;
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
  public async Task<Results<Ok<GetWhiteCompaniesResponse.Ok>, BadRequest>> GetWhiteCompanies(
    [FromServices] GetWhiteCompaniesEndpoint endpoint
  ) =>
    await endpoint.Handle() switch
    {
      GetWhiteCompaniesResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost]
  [WithAdminAccess(nameof(AdminPermissions.white_companies_write))]
  public async Task<Results<Ok<AddCompanyResponse.Ok>, BadRequest>> AddCompany(
    [FromServices] AddCompanyEndpoint endpoint,
    [FromBody] AddCompanyRequest req
  ) =>
    await endpoint.Handle(req) switch
    {
      AddCompanyResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
