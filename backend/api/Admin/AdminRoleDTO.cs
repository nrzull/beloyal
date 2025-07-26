namespace Beloyal.API.Admin;

public record AdminRoleDTO(Guid Id, string Name, DateTimeOffset CreatedAt, List<string> Permissions)
{
  public static AdminRoleDTO From(AdminRole v) =>
    new(Id: v.Id, Name: v.Name, CreatedAt: v.CreatedAt, Permissions: v.Permissions);
};
