using Beloyal.API.Admin;

namespace Beloyal.API.Web.AdminRoles.CreateRole;

public class CreateRoleEndpoint(AdminContext adminContext)
{
  public async Task<CreateRoleResponse> Handle(CreateRoleRequest req)
  {
    var role = await adminContext.CreateAdminRole(req.Name, req.Permissions);
    return new CreateRoleResponse.Ok(AdminRoleDTO.From(role));
  }
}
