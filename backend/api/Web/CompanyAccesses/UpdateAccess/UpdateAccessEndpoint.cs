using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccesses.UpdateAccess;

public class UpdateAccessEndpoint(CompanyContext companyContext)
{
  public async Task<UpdateAccessResponse> Handle(
    CompanyAccess companyAccess,
    UpdateAccessRequest req
  )
  {
    var company = await companyContext.GetCompany(companyAccess);

    var role = await companyContext.GetRole(company, req.RoleId);
    if (role is null)
    {
      throw new Exception("invalid role");
    }

    var access = await companyContext.GetAccess(req.CompanyAccessId);
    if (access is null || companyAccess.CompanyId != access.CompanyId)
    {
      throw new Exception("invalid access");
    }

    await companyContext.UpdateCompanyAccess(access, role);

    return new UpdateAccessResponse.Ok(CompanyAccessDTO.From(access));
  }
}
