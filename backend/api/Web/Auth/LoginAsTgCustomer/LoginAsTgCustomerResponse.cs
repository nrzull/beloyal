using Beloyal.API.Users;

namespace Beloyal.API.Web.Auth.LoginAsTgCustomer;

public abstract record LoginAsTgCustomerResponse
{
  public record Ok(UserDTO user) : LoginAsTgCustomerResponse;

  public record InvalidUser : LoginAsTgCustomerResponse;
}
