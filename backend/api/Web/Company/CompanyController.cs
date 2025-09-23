using Beloyal.API.Companies;
using Beloyal.API.Web.Company.GetCashback;
using Beloyal.API.Web.Company.GetPublicLinks;
using Beloyal.API.Web.Company.SetCashback;
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
  [HttpGet("cashback")]
  public async Task<Results<Ok<GetCashbackResponse.Ok>, BadRequest>> GetCashback(
    [FromServices] GetCashbackEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetCashbackResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("cashback")]
  public async Task<Results<Ok<SetCashbackResponse.Ok>, BadRequest>> SetCashback(
    [FromServices] SetCashbackEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CompanyCashbackDTO req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      SetCashbackResponse.Ok ok => TypedResults.Ok(ok),
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
