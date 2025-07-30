using Beloyal.API.Admin;

namespace Beloyal.API.Web.AdminRoles.CreateRole;

public abstract record CreateRoleResponse
{
  public record Ok(AdminRoleDTO Data) : CreateRoleResponse;
}
