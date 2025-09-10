using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Messengers;
using Beloyal.API.Shops;
using Vogen;

[ValueObject<Guid>]
public partial struct CompanyId
{
  public static CompanyId From(Company v) => From(v.Id);

  public static CompanyId From(CompanyAccess v) => From(v.CompanyId);

  public static CompanyId From(Shop v) => From(v.CompanyId);

  public static CompanyId From(TgBot v) => From(v.CompanyId);

  public static CompanyId From(Customer v) => From(v.CompanyId);
};
