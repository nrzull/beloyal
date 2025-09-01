using Beloyal.API.Companies;

namespace Beloyal.API.Mailings;

public class Mailing
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public required string Content { get; set; }

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }

  public List<MailingUnit> Units { get; set; } = [];
}
