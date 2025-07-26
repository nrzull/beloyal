using Beloyal.API.Admin;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.GetSelf;

public class GetSelfEndpoint(AdminContext adminContext)
{
  public async Task<GetSelfResponse> Handle(User user)
  {
    var access = await adminContext.GetAccess(user);

    return new GetSelfResponse.Ok(
      UserDTO.From(user),
      access is { } ? AdminAccessDTO.From(access) : null
    );
  }
}
