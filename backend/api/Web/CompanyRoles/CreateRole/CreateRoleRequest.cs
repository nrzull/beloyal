namespace Beloyal.API.Web.CompanyRoles.CreateRole;

public record CreateRoleRequest(string Name, List<string> Permissions);
