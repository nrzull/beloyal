using Beloyal.API.Chats;
using Beloyal.API.Customers;

namespace Beloyal.API.Mailings;

public class MailingUnit
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public DateTimeOffset? UpdatedAt { get; set; }
  public MailingUnitStatus Status { get; set; } = MailingUnitStatus.New;

  public Guid MailingId { get; set; }
  public required Mailing Mailing { get; set; }

  public Guid CustomerId { get; set; }
  public required Customer Customer { get; set; }

  public ChatMessage? ChatMessage { get; set; }
}
