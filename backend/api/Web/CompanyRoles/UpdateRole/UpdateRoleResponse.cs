using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyRoles.UpdateRole;

public abstract record UpdateRoleResponse
{
  public record Ok(CompanyRoleDTO Data) : UpdateRoleResponse;
}
