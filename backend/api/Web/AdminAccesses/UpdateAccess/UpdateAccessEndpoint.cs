using Beloyal.API.Admin;
using Beloyal.API.Users;

namespace Beloyal.API.Web.AdminAccesses.UpdateAccess;

public class UpdateAccessEndpoint(AdminContext adminContext, UserContext userContext)
{
  public async Task<UpdateAccessResponse> Handle(UpdateAccessRequest req)
  {
    var role = await adminContext.GetRole(req.RoleId);
    if (role is null)
    {
      throw new Exception("invalid role");
    }

    var user = await userContext.GetById(req.UserId);

    var access = await adminContext.GetAccess(user);
    if (access is null)
    {
      throw new Exception("invalid access");
    }

    await adminContext.UpdateAdminAccess(access, role);

    return new UpdateAccessResponse.Ok(AdminAccessDTO.From(access));
  }
}
