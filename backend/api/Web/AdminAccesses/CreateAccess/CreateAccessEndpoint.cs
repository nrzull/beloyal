using Beloyal.API.Admin;
using Beloyal.API.Users;

namespace Beloyal.API.Web.AdminAccesses.CreateAccess;

public class CreateAccessEndpoint(AdminContext adminContext, UserContext userContext)
{
  public async Task<CreateAccessResponse> Handle(CreateAccessRequest req)
  {
    var role = await adminContext.GetRole(req.RoleId);
    if (role is null)
    {
      throw new Exception("invalid role");
    }

    var user = await userContext.GetById(req.UserId);
    var access = await adminContext.CreateAdminAccess(user, role);

    return new CreateAccessResponse.Ok(AdminAccessDTO.From(access));
  }
}
