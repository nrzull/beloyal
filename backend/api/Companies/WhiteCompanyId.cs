using Vogen;

namespace Beloyal.API.Companies;

[ValueObject<Guid>]
public partial struct WhiteCompanyId
{
  public static WhiteCompanyId From(WhiteCompany v) => From(v.Id);
};
