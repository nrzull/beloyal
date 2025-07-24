namespace Beloyal.API.Companies;

public class WhiteCompany
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;

  public required string Tin { get; set; }
  public required DateTimeOffset ExpiredAt { get; set; }
}
