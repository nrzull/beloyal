using Beloyal.API.Admin;

namespace Beloyal.API.Web.AdminRoles.UpdateRole;

public abstract record UpdateRoleResponse
{
  public record Ok(AdminRoleDTO Data) : UpdateRoleResponse;
}
