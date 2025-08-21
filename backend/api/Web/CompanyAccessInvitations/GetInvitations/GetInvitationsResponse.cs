using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccessInvitations.GetInvitations;

public abstract record GetInvitationsResponse
{
  public record Ok(List<CompanyAccessInvitationDTO> Data) : GetInvitationsResponse;
}
