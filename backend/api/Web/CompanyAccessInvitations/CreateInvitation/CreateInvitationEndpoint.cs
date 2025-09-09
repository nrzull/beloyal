using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccessInvitations.CreateInvitation;

public class CreateInvitationEndpoint(CompanyContext companyContext)
{
  public async Task<CreateInvitationResponse> Handle(
    CompanyAccess access,
    CreateInvitationRequest req
  )
  {
    var company = await companyContext.GetCompany(CompanyId.From(access));
    var role = await companyContext.GetRole(
      CompanyId.From(company),
      CompanyRoleId.From(req.RoleId)
    );
    if (role is null)
    {
      throw new Exception("invalid role");
    }

    var invitation = await companyContext.CreateCompanyAccessInvitation(
      company,
      role,
      new(req.Phone)
    );

    return new CreateInvitationResponse.Ok(CompanyAccessInvitationDTO.From(invitation));
  }
}
