using Beloyal.API.Auth;
using Beloyal.API.Types;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Auth.Confirm;

public class ConfirmEndpoint(AuthContext authContext, UserContext userContext)
{
  public async Task<ConfirmResponse> Handle(Email email, string code)
  {
    var otp = await authContext.DrainEmailOTP(email, code);
    if (otp is null)
    {
      throw new Exception("invalid otp");
    }

    var user = await userContext.EnsureUser(email);
    var token = authContext.EncodeToken(user);

    return new ConfirmResponse.Ok(token);
  }
}
