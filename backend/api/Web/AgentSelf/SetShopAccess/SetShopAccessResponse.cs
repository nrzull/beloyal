using Beloyal.API.Auth;

namespace Beloyal.API.Web.AgentSelf.SetShopAccess;

public abstract record SetShopAccessResponse
{
  public record Ok(EncodedAccessToken AccessToken, EncodedRefreshToken RefreshToken)
    : SetShopAccessResponse;
}
