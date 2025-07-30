using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyRoles.UpdateRole;

public class UpdateRoleEndpoint(CompanyContext companyContext)
{
  public async Task<UpdateRoleResponse> Handle(CompanyAccess access, UpdateRoleRequest req)
  {
    var company = await companyContext.GetCompany(access.CompanyId);

    var role = await companyContext.GetRole(company, req.Id);
    if (role is null)
    {
      throw new Exception("invalid role");
    }

    await companyContext.UpdateCompanyRole(role, req.Name, req.Permissions);

    return new UpdateRoleResponse.Ok(CompanyRoleDTO.From(role));
  }
}
