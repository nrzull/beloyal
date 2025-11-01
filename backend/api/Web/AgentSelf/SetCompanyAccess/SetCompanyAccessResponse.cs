using Beloyal.API.Auth;

namespace Beloyal.API.Web.AgentSelf.SetCompanyAccess;

public abstract record SetCompanyAccessResponse
{
  public record Ok(EncodedAccessToken AccessToken, EncodedRefreshToken RefreshToken)
    : SetCompanyAccessResponse;
}
