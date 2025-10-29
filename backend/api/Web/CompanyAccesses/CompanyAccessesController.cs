using Beloyal.API.Companies;
using Beloyal.API.Web.CompanyAccesses.BlockAccess;
using Beloyal.API.Web.CompanyAccesses.CreateAccess;
using Beloyal.API.Web.CompanyAccesses.GetAccess;
using Beloyal.API.Web.CompanyAccesses.GetAccesses;
using Beloyal.API.Web.CompanyAccesses.UnblockAccess;
using Beloyal.API.Web.CompanyAccesses.UpdateAccess;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.CompanyAccesses;

[ApiController]
[Route("/api/company/company-accesses")]
[WithAuth]
[WithCompanyAccess]
public class CompanyAccessesController : ControllerBase
{
  [HttpGet]
  [WithCompanyPermissions(nameof(CompanyPermissions.company_accesses_read))]
  public async Task<Results<Ok<GetAccessesResponse.Ok>, BadRequest>> GetCompanyAccesses(
    [FromServices] GetAccessesEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetAccessesResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost]
  [WithCompanyPermissions(nameof(CompanyPermissions.company_accesses_write))]
  public async Task<Results<Ok<CreateAccessResponse.Ok>, BadRequest>> CreateCompanyAccess(
    [FromServices] CreateAccessEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateAccessRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateAccessResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}")]
  [WithCompanyPermissions(nameof(CompanyPermissions.company_accesses_read))]
  public async Task<Results<NotFound, Ok<GetAccessResponse.Ok>, BadRequest>> GetAccess(
    [FromServices] GetAccessEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid companyAccessId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), CompanyAccessId.From(companyAccessId)) switch
    {
      GetAccessResponse.NotFound => TypedResults.NotFound(),
      GetAccessResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("{id}")]
  [WithCompanyPermissions(nameof(CompanyPermissions.company_accesses_write))]
  public async Task<Results<Ok<UpdateAccessResponse.Ok>, BadRequest>> UpdateCompanyAccess(
    [FromServices] UpdateAccessEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] UpdateAccessRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      UpdateAccessResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("{id}/block")]
  [WithCompanyPermissions(nameof(CompanyPermissions.company_accesses_write))]
  public async Task<Results<Ok<BlockAccessResponse.Ok>, BadRequest>> BlockCompanyAccess(
    [FromServices] BlockAccessEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid companyAccessId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), CompanyAccessId.From(companyAccessId)) switch
    {
      BlockAccessResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("{id}/unblock")]
  [WithCompanyPermissions(nameof(CompanyPermissions.company_accesses_write))]
  public async Task<Results<Ok<UnblockAccessResponse.Ok>, BadRequest>> UnblockCompanyAccess(
    [FromServices] UnblockAccessEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid companyAccessId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), CompanyAccessId.From(companyAccessId)) switch
    {
      UnblockAccessResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
