namespace Beloyal.API.Web.Auth.Login;

public abstract record LoginResponse
{
  public record Ok(DateTimeOffset ExpiredAt) : LoginResponse;

  public record UserNotFound : LoginResponse;
}
