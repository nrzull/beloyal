using Beloyal.API.Companies;
using Beloyal.API.Web.CompanyRoles.CreateRole;
using Beloyal.API.Web.CompanyRoles.GetPermissions;
using Beloyal.API.Web.CompanyRoles.GetRole;
using Beloyal.API.Web.CompanyRoles.GetRoles;
using Beloyal.API.Web.CompanyRoles.UpdateRole;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.CompanyRoles;

[ApiController]
[Route("/api/company/company-roles")]
[WithAuth]
[WithCompanyAccess]
public class CompanyRolesController : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetRoles(
    [FromServices] GetRolesEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetRolesResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpGet("{id}")]
  public async Task<IActionResult> GetRole(
    [FromServices] GetRoleEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid roleId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), roleId) switch
    {
      GetRoleResponse.NotFound => NotFound(),
      GetRoleResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpPost]
  public async Task<IActionResult> CreateRole(
    [FromServices] CreateRoleEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateRoleRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateRoleResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpPatch("{id}")]
  public async Task<IActionResult> UpdateRole(
    [FromServices] UpdateRoleEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] UpdateRoleRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      UpdateRoleResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpGet("permissions")]
  public IActionResult GetPermissions([FromServices] GetPermissionsEndpoint endpoint) =>
    endpoint.Handle() switch
    {
      GetPermissionsResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
