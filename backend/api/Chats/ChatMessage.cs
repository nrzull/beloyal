using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Mailings;

namespace Beloyal.API.Chats;

public class ChatMessage
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public DateTimeOffset? ReadAt { get; set; }

  public string? Content { get; set; }
  public List<string>? AttachmentKeys { get; set; }

  public Guid? CompanyId { get; set; }
  public Company? Company { get; set; }

  public Guid? CustomerId { get; set; }
  public Customer? Customer { get; set; }

  public Guid? AgentId { get; set; }
  public CompanyAccess? Agent { get; set; }

  public Guid ChatId { get; set; }
  public required Chat Chat { get; set; }

  public Guid? MailingUnitId { get; set; }
  public MailingUnit? MailingUnit { get; set; }
}
