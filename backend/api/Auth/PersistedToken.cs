namespace Beloyal.API.Auth;

public class PersistedToken
{
  public Guid UserId { get; set; }
  public required string Hash { get; set; }
  public required DateTimeOffset IssuedAt { get; set; }
}
