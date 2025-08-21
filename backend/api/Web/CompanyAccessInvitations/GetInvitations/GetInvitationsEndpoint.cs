using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccessInvitations.GetInvitations;

public class GetInvitationsEndpoint(CompanyContext companyContext)
{
  public async Task<GetInvitationsResponse> Handle(CompanyAccess access)
  {
    var invitations = await companyContext.GetCompanyAccessInvitations(
      CompanyId.From(access.CompanyId)
    );

    return new GetInvitationsResponse.Ok(
      invitations.Select(CompanyAccessInvitationDTO.From).ToList()
    );
  }
}
