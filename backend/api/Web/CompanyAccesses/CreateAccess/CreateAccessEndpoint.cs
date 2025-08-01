using Beloyal.API.Companies;
using Beloyal.API.Users;

namespace Beloyal.API.Web.CompanyAccesses.CreateAccess;

public class CreateAccessEndpoint(CompanyContext companyContext, UserContext userContext)
{
  public async Task<CreateAccessResponse> Handle(
    CompanyAccess companyAccess,
    CreateAccessRequest req
  )
  {
    var company = await companyContext.GetCompany(CompanyId.From(companyAccess));

    var role = await companyContext.GetRole(
      CompanyId.From(company),
      CompanyRoleId.From(req.RoleId)
    );
    if (role is null)
    {
      throw new Exception("invalid role");
    }

    var user = await userContext.GetById(UserId.From(req.UserId));
    var access = await companyContext.CreateCompanyAccess(company, user, role);

    return new CreateAccessResponse.Ok(CompanyAccessDTO.From(access));
  }
}
