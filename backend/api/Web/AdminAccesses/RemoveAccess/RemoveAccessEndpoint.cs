using Beloyal.API.Admin;
using Beloyal.API.Users;

namespace Beloyal.API.Web.AdminAccesses.RemoveAccess;

public class RemoveAccessEndpoint(AdminContext adminContext, UserContext userContext)
{
  public async Task<RemoveAccessResponse> Handle(RemoveAccessRequest req)
  {
    var user = await userContext.GetById(req.UserId);
    var access = await adminContext.GetAccess(user);
    if (access is null)
    {
      throw new Exception("invalid access");
    }

    await adminContext.RemoveAdminAccess(access);

    return new RemoveAccessResponse.Ok();
  }
}
