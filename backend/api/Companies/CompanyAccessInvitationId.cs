using Vogen;

namespace Beloyal.API.Companies;

[ValueObject<Guid>]
public partial struct CompanyAcessInvitationId
{
  public static CompanyAcessInvitationId From(CompanyAccessInvitation v) => From(v.Id);
};
