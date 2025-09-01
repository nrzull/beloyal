namespace Beloyal.API.Chats;

public record ChatDTO(Guid Id, ChatMessageDTO? PreviewMessage)
{
  public static ChatDTO From(Chat v) =>
    new(
      Id: v.Id,
      PreviewMessage: new Func<ChatMessageDTO?>(() =>
      {
        var lastMessage = v.Frames?.FirstOrDefault()?.Messages?.FirstOrDefault();

        return lastMessage is { } l ? ChatMessageDTO.From(l) : null;
      })()
    );
};
