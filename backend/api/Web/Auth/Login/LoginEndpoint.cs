using Beloyal.API.Auth;
using Beloyal.API.Types;

namespace Beloyal.API.Web.Auth.Login;

public class LoginEndpoint(AuthContext authContext, AuthTgBot authTgBot)
{
  public async Task<LoginResponse> Handle(Phone phone)
  {
    var result = await authContext.CreateOtp(phone);
    return await authTgBot.SendCode(result.Phone, result.Code) switch
    {
      AuthTgBot.SendMessageResult.UserNotFound => new LoginResponse.UserNotFound(),
      AuthTgBot.SendMessageResult.Ok => new LoginResponse.Ok(result.ExpiredAt),
      _ => throw new Exception("LoginEndpoint. unhandled clause"),
    };
  }
}
