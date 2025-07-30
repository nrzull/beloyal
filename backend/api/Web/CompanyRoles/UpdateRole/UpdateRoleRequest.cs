namespace Beloyal.API.Web.CompanyRoles.UpdateRole;

public record UpdateRoleRequest(Guid Id, string Name, List<string> Permissions);
