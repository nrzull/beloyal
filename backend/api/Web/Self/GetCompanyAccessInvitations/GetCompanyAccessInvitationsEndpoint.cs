using Beloyal.API.Companies;
using Beloyal.API.Types;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.GetCompanyAccessInvitations;

public class GetCompanyAccessInvitationsEndpoint(CompanyContext companyContext)
{
  public async Task<GetCompanyAccessInvitationsResponse> Handle(User user)
  {
    if (user.Email is null)
    {
      throw new Exception("no email");
    }

    var invitations = await companyContext.GetCompanyAccessInvitations(new Email(user.Email));

    return new GetCompanyAccessInvitationsResponse.Ok(
      invitations.Select(CompanyAccessInvitationDTO.From).ToList()
    );
  }
}
