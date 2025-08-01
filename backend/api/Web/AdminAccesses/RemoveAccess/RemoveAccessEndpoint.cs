using Beloyal.API.Admin;
using Beloyal.API.Users;

namespace Beloyal.API.Web.AdminAccesses.RemoveAccess;

public class RemoveAccessEndpoint(AdminContext adminContext)
{
  public async Task<RemoveAccessResponse> Handle(RemoveAccessRequest req)
  {
    var access = await adminContext.GetAccess(UserId.From(req.UserId));
    if (access is null)
    {
      throw new Exception("invalid access");
    }

    await adminContext.RemoveAdminAccess(access);

    return new RemoveAccessResponse.Ok();
  }
}
