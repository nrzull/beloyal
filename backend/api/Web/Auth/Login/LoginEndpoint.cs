using Beloyal.API.Auth;
using Beloyal.API.Types;

namespace Beloyal.API.Web.Auth.Login;

public class LoginEndpoint(AuthContext authContext)
{
  public async Task<LoginResponse> Handle(Email email)
  {
    var result = await authContext.CreateEmailOTP(email);
    return new LoginResponse.Ok(result.Code, result.ExpiredAt);
  }
}
