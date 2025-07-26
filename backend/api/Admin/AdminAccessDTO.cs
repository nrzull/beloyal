namespace Beloyal.API.Admin;

public record AdminAccessDTO(Guid Id, AdminRoleDTO Role)
{
  public static AdminAccessDTO From(AdminAccess v) =>
    new(Id: v.Id, Role: AdminRoleDTO.From(v.Role));
};
