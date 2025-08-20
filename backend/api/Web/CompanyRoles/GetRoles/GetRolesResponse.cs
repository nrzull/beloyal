using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyRoles.GetRoles;

public abstract record GetRolesResponse
{
  public record Ok(List<CompanyRoleDTO> Data) : GetRolesResponse;
}
