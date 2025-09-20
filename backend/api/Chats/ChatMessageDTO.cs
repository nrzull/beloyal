namespace Beloyal.API.Chats;

public record ChatMessageDTO(
  Guid Id,
  DateTimeOffset CreatedAt,
  DateTimeOffset? ReadAt,
  string? Content,
  Guid? AgentId,
  Guid? CustomerId,
  Guid ChatId,
  string Name
)
{
  public static ChatMessageDTO From(ChatMessage v) =>
    new(
      Id: v.Id,
      CreatedAt: v.CreatedAt,
      ReadAt: v.ReadAt,
      AgentId: v.AgentId,
      CustomerId: v.CustomerId,
      ChatId: v.ChatId,
      Content: v.Content,
      Name: v.Agent is { } g ? g.User!.Phone!
        : v.Customer is { } c ? c.User!.Phone!
        : throw new Exception("no agent nor customer")
    );
};
