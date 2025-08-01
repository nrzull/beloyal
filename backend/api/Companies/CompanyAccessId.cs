using Vogen;

namespace Beloyal.API.Companies;

[ValueObject<Guid>]
public partial struct CompanyAccessId
{
  public static CompanyAccessId From(CompanyAccess v) => From(v.Id);
};
