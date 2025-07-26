using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.GetSelf;

public class GetSelfEndpoint
{
  public GetSelfResponse Handle(User user)
  {
    return new GetSelfResponse.Ok(user.Id);
  }
}
