using Beloyal.API.Auth;

namespace Beloyal.API.Web.Auth.GetLoginVariants;

public class GetLoginVariantsEndpoint(AuthTgBot authTgBot)
{
  public GetLoginVariantsResponse Handle()
  {
    return new GetLoginVariantsResponse.Ok(
      [new GetLoginVariantsResponse.OkData("telegram", authTgBot.GetUsername())]
    );
  }
}
