namespace Beloyal.API.Auth;

public class AuthEmailOTP
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public required string Email { get; set; }
  public required string Code { get; set; }
  public required DateTimeOffset ExpiredAt { get; set; }
}
