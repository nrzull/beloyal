using Beloyal.API.Auth;

namespace Beloyal.API.Web.Auth.LoginAsTgCustomer;

public abstract record LoginAsTgCustomerResponse
{
  public record Ok(OkData Data) : LoginAsTgCustomerResponse;

  public record OkData(EncodedAccessToken AuthToken, EncodedAccessToken PublicToken);

  public record InvalidUser : LoginAsTgCustomerResponse;
}
