namespace Beloyal.API.Web.CompanyRoles.GetPermissions;

public abstract record GetPermissionsResponse
{
  public record Ok(List<string> Data) : GetPermissionsResponse;
}
