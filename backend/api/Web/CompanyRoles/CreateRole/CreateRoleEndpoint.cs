using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyRoles.CreateRole;

public class CreateRoleEndpoint(CompanyContext companyContext)
{
  public async Task<CreateRoleResponse> Handle(CompanyAccess access, CreateRoleRequest req)
  {
    var company = await companyContext.GetCompany(CompanyId.From(access));

    var role = await companyContext.CreateCompanyRole(company, req.Name, req.Permissions);

    return new CreateRoleResponse.Ok(CompanyRoleDTO.From(role));
  }
}
