using Beloyal.API.Web.CompanyRoles.CreateRole;
using Beloyal.API.Web.CompanyRoles.GetPermissions;
using Beloyal.API.Web.CompanyRoles.GetRoles;
using Beloyal.API.Web.CompanyRoles.UpdateRole;

namespace Beloyal.API.Web.CompanyRoles;

public static class CompanyRolesExtensions
{
  public static void UseCompanyRolesEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<CreateRoleEndpoint>();
    builder.Services.AddScoped<UpdateRoleEndpoint>();
    builder.Services.AddScoped<GetRolesEndpoint>();
    builder.Services.AddScoped<GetPermissionsEndpoint>();
  }
}
