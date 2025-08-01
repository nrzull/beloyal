using Vogen;

namespace Beloyal.API.Companies;

[ValueObject<Guid>]
public partial struct CompanyRoleId
{
  public static CompanyRoleId From(CompanyRole v) => From(v.Id);

  public static CompanyRoleId From(CompanyAccessInvitation v) => From(v.RoleId);
};
