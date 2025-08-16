using Beloyal.API.Companies;

namespace Beloyal.API.Web.Self.GetCompanyAccessInvitations;

public abstract record GetCompanyAccessInvitationsResponse
{
  public record Ok(List<CompanyAccessInvitationDTO> Data) : GetCompanyAccessInvitationsResponse;
}
