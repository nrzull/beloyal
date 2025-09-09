using Beloyal.API.Auth;
using Beloyal.API.Types;

namespace Beloyal.API.Web.Auth.Login;

public class LoginEndpoint(AuthContext authContext)
{
  public async Task<LoginResponse> Handle(Phone phone)
  {
    var result = await authContext.CreateOtp(phone);
    return new LoginResponse.Ok(result.Code, result.ExpiredAt);
  }
}
