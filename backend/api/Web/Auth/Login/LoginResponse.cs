namespace Beloyal.API.Web.Auth.Login;

public abstract record LoginResponse
{
  public record Ok(string Code, DateTimeOffset ExpiredAt) : LoginResponse;
}
