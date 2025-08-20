using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyRoles.GetRoles;

public class GetRolesEndpoint(CompanyContext companyContext)
{
  public async Task<GetRolesResponse> Handle(CompanyAccess access)
  {
    var roles = await companyContext.GetCompanyRoles(CompanyId.From(access.CompanyId));

    return new GetRolesResponse.Ok(roles.Select(CompanyRoleDTO.From).ToList());
  }
}
