namespace Beloyal.API.Users;

public class User
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
  public required string Email { get; set; }
}
