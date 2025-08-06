using Beloyal.API.Companies;
using Beloyal.API.Customers;

namespace Beloyal.API.Chats;

public class Chat
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  public Guid CompanyId { get; set; }
  public required Company Company { get; set; }

  public Guid CustomerId { get; set; }
  public required Customer Customer { get; set; }

  public List<ChatFrame> Frames { get; set; } = [];
}
