using Beloyal.API.Companies;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.AcceptCompanyAccessInvitation;

public class AcceptCompanyAccessInvitationEndpoint(
  CompanyContext companyContext,
  ApplicationDbContext dbContext
)
{
  public async Task<AcceptCompanyAccessInvitationResponse> Handle(
    User user,
    AcceptCompanyAccessInvitationRequest req
  )
  {
    var invitation = await companyContext.GetCompanyAccessInvitation(user, req.Id);
    if (invitation is null)
    {
      throw new Exception("invalid invitation");
    }

    var company = await companyContext.GetCompany(invitation.CompanyId);
    var role = await companyContext.GetRole(company, invitation.RoleId);
    if (role is null)
    {
      throw new Exception("invalid role");
    }

    await using var tc = await dbContext.MaybeTransactionAsync();

    var access = await companyContext.CreateCompanyAccess(company, user, role);

    await companyContext.UpdateCompanyAccessInvitation(
      invitation,
      CompanyAccessInvitationStatus.Accepted
    );

    await tc.CommitAsync();

    return new AcceptCompanyAccessInvitationResponse.Ok(CompanyAccessDTO.From(access));
  }
}
