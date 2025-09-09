namespace Beloyal.API.Auth;

public class AuthOtp
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public required string Phone { get; set; }
  public required string Code { get; set; }
  public required DateTimeOffset ExpiredAt { get; set; }
}
