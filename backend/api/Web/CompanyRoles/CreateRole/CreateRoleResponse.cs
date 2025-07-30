using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyRoles.CreateRole;

public abstract record CreateRoleResponse
{
  public record Ok(CompanyRoleDTO Data) : CreateRoleResponse;
}
