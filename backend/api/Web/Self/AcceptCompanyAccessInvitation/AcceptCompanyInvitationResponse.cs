using Beloyal.API.Companies;

namespace Beloyal.API.Web.Self.AcceptCompanyAccessInvitation;

public abstract record AcceptCompanyAccessInvitationResponse
{
  public record Ok(CompanyAccessDTO Data) : AcceptCompanyAccessInvitationResponse;
}
