using Beloyal.API.Companies;
using Beloyal.API.Messengers;
using Vogen;

[ValueObject<Guid>]
public partial struct CompanyId
{
  public static CompanyId From(Company v) => From(v.Id);

  public static CompanyId From(CompanyAccess v) => From(v.CompanyId);

  public static CompanyId From(TgBot v) => From(v.CompanyId);
};
