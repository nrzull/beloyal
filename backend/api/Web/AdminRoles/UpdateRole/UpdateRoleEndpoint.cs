using Beloyal.API.Admin;

namespace Beloyal.API.Web.AdminRoles.UpdateRole;

public class UpdateRoleEndpoint(AdminContext adminContext)
{
  public async Task<UpdateRoleResponse> Handle(UpdateRoleRequest req)
  {
    var role = await adminContext.GetRole(req.Id);
    if (role is null)
    {
      throw new Exception("invalid role");
    }

    await adminContext.UpdateAdminRole(role, req.Name, req.Permissions);

    return new UpdateRoleResponse.Ok(AdminRoleDTO.From(role));
  }
}
