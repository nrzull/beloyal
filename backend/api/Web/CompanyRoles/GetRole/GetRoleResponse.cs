using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyRoles.GetRole;

public abstract record GetRoleResponse
{
  public record Ok(CompanyRoleDTO Data) : GetRoleResponse;

  public record NotFound : GetRoleResponse;
}
