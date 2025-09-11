using Beloyal.API.Companies;
using Beloyal.API.Web.CompanyRoles.CreateRole;
using Beloyal.API.Web.CompanyRoles.GetPermissions;
using Beloyal.API.Web.CompanyRoles.GetRole;
using Beloyal.API.Web.CompanyRoles.GetRoles;
using Beloyal.API.Web.CompanyRoles.UpdateRole;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.CompanyRoles;

[ApiController]
[Route("/api/company/company-roles")]
[WithAuth]
[WithCompanyAccess]
public class CompanyRolesController : ControllerBase
{
  [HttpGet]
  public async Task<Results<Ok<GetRolesResponse.Ok>, BadRequest>> GetRoles(
    [FromServices] GetRolesEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetRolesResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}")]
  public async Task<Results<Ok<GetRoleResponse.Ok>, NotFound, BadRequest>> GetRole(
    [FromServices] GetRoleEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid roleId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), roleId) switch
    {
      GetRoleResponse.NotFound => TypedResults.NotFound(),
      GetRoleResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost]
  public async Task<Results<Ok<CreateRoleResponse.Ok>, BadRequest>> CreateRole(
    [FromServices] CreateRoleEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateRoleRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateRoleResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("{id}")]
  public async Task<Results<Ok<UpdateRoleResponse.Ok>, BadRequest>> UpdateRole(
    [FromServices] UpdateRoleEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] UpdateRoleRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      UpdateRoleResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("permissions")]
  public Results<Ok<GetPermissionsResponse.Ok>, BadRequest> GetPermissions(
    [FromServices] GetPermissionsEndpoint endpoint
  ) =>
    endpoint.Handle() switch
    {
      GetPermissionsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
