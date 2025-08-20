using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyRoles.GetRole;

public class GetRoleEndpoint(CompanyContext companyContext)
{
  public async Task<GetRoleResponse> Handle(CompanyAccess access, Guid id)
  {
    var result = await companyContext.GetRole(
      CompanyId.From(access.CompanyId),
      CompanyRoleId.From(id)
    );

    if (result is null)
    {
      return new GetRoleResponse.NotFound();
    }

    return new GetRoleResponse.Ok(CompanyRoleDTO.From(result));
  }
}
