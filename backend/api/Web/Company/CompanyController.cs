using Beloyal.API.Companies;
using Beloyal.API.Web.Company.GetBonusOptions;
using Beloyal.API.Web.Company.GetPublicLinks;
using Beloyal.API.Web.Company.SetBonusOptions;
using Beloyal.API.Web.Company.UpdateCompany;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Company;

[ApiController]
[Route("/api/company")]
[WithAuth]
[WithCompanyAccess]
public class CompanyController : ControllerBase
{
  [HttpPatch]
  [WithCompanyPermissions(nameof(CompanyPermissions.company_current_write))]
  public async Task<Results<Ok<UpdateCompanyResponse.Ok>, BadRequest>> UpdateCompany(
    [FromServices] UpdateCompanyEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] UpdateCompanyRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      UpdateCompanyResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("cashback")]
  [WithCompanyPermissions(nameof(CompanyPermissions.company_options_read))]
  public async Task<Results<Ok<GetBonusOptionsResponse.Ok>, BadRequest>> GetBonusOptions(
    [FromServices] GetBonusOptionsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetBonusOptionsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("cashback")]
  [WithCompanyPermissions(nameof(CompanyPermissions.company_options_write))]
  public async Task<Results<Ok<SetBonusOptionsResponse.Ok>, BadRequest>> SetBonusOptions(
    [FromServices] SetBonusOptionsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CompanyBonusOptionsDTO req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      SetBonusOptionsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("links")]
  public async Task<Results<Ok<GetPublicLinksResponse.Ok>, BadRequest>> GetCompanyLinks(
    [FromServices] GetPublicLinksEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(CompanyId.From(companyAccess.Ensure())) switch
    {
      GetPublicLinksResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
