using Beloyal.API.Web.AdminRoles.CreateRole;
using Beloyal.API.Web.AdminRoles.UpdateRole;

namespace Beloyal.API.Web.AdminRoles;

public static class AdminRolesExtensions
{
  public static void UseAdminRolesEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<CreateRoleEndpoint>();
    builder.Services.AddScoped<UpdateRoleEndpoint>();
  }
}
