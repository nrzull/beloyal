using Beloyal.API.Auth;

namespace Beloyal.API.Web.Auth.Confirm;

public abstract record ConfirmResponse
{
  public record Ok(EncodedAccessToken AccessToken, EncodedRefreshToken RefreshToken)
    : ConfirmResponse;
}
