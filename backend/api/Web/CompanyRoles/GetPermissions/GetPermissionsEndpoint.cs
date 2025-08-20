using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyRoles.GetPermissions;

public class GetPermissionsEndpoint
{
  public GetPermissionsResponse Handle()
  {
    return new GetPermissionsResponse.Ok(CompanyPermissions.GetAllPermissions());
  }
}
