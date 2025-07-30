namespace Beloyal.API.Web.AdminRoles.CreateRole;

public record CreateRoleRequest(string Name, List<string> Permissions);
