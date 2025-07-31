using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccessInvitations.CreateInvitation;

public abstract record CreateInvitationResponse
{
  public record Ok(CompanyAccessInvitationDTO Data) : CreateInvitationResponse;
}
