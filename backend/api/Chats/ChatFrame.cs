using Beloyal.API.Companies;

namespace Beloyal.API.Chats;

public class ChatFrame
{
  public Guid Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public required ChatFrameType Type { get; set; }
  public required ChatFrameStatus Status { get; set; }

  public int? Rate { get; set; }
  public string? RateComment { get; set; }

  public Guid ChatId { get; set; }
  public required Chat Chat { get; set; }

  public Guid? AgentId { get; set; }
  public CompanyAccess? Agent { get; set; }

  public List<ChatMessage> Messages { get; set; } = [];
}
