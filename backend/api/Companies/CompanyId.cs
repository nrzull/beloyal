using Beloyal.API.Companies;
using Vogen;

[ValueObject<Guid>]
public partial struct CompanyId
{
  public static CompanyId From(Company v) => From(v.Id);

  public static CompanyId From(CompanyAccess v) => From(v.CompanyId);
};
