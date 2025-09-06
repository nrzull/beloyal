using Beloyal.API.Auth;

namespace Beloyal.API.Web.Auth.LoginAsTgCustomer;

public abstract record LoginAsTgCustomerResponse
{
  public record Ok(EncodedAccessToken Data) : LoginAsTgCustomerResponse;

  public record InvalidUser : LoginAsTgCustomerResponse;
}
