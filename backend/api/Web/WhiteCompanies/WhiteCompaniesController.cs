using Beloyal.API.Admin;
using Beloyal.API.Companies;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.WhiteCompanies.AddCompany;
using Beloyal.API.Web.WhiteCompanies.GetWhiteCompanies;
using Beloyal.API.Web.WhiteCompanies.GetWhiteCompany;
using Beloyal.API.Web.WhiteCompanies.UpdateCompany;
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
  [WithAdminPermissions(nameof(AdminPermissions.white_companies_read))]
  public async Task<Results<Ok<GetWhiteCompaniesResponse.Ok>, BadRequest>> GetWhiteCompanies(
    [FromServices] GetWhiteCompaniesEndpoint endpoint
  ) =>
    await endpoint.Handle() switch
    {
      GetWhiteCompaniesResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}")]
  [WithAdminPermissions(nameof(AdminPermissions.white_companies_read))]
  public async Task<Results<Ok<GetWhiteCompanyResponse.Ok>, NotFound, BadRequest>> GetWhiteCompany(
    [FromServices] GetWhiteCompanyEndpoint endpoint,
    [FromRoute(Name = "id")] Guid id
  ) =>
    await endpoint.Handle(WhiteCompanyId.From(id)) switch
    {
      GetWhiteCompanyResponse.NotFound => TypedResults.NotFound(),
      GetWhiteCompanyResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("{id}")]
  [WithAdminPermissions(nameof(AdminPermissions.white_companies_write))]
  public async Task<Results<Ok<UpdateCompanyResponse.Ok>, BadRequest>> UpdateCompany(
    [FromServices] UpdateCompanyEndpoint endpoint,
    [FromRoute(Name = "id")] Guid id,
    [FromBody] UpdateCompanyRequest req
  ) =>
    await endpoint.Handle(WhiteCompanyId.From(id), req) switch
    {
      UpdateCompanyResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost]
  [WithAdminPermissions(nameof(AdminPermissions.white_companies_write))]
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
