namespace Beloyal.API.Web.AdminRoles.UpdateRole;

public record UpdateRoleRequest(Guid Id, string Name, List<string> Permissions);
