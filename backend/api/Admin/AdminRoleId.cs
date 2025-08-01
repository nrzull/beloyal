using Vogen;

namespace Beloyal.API.Admin;

[ValueObject<Guid>]
public partial struct AdminRoleId
{
  public static AdminRoleId From(AdminRole v) => From(v.Id);
};
