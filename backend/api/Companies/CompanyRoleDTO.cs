namespace Beloyal.API.Companies;

public record CompanyRoleDTO(
  Guid Id,
  string Name,
  DateTimeOffset CreatedAt,
  List<string> Permissions
)
{
  public static CompanyRoleDTO From(CompanyRole v) =>
    new(Id: v.Id, Name: v.Name, CreatedAt: v.CreatedAt, Permissions: v.Permissions);
};
