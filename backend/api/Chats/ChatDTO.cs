using Amazon.S3;

namespace Beloyal.API.Chats;

public record ChatDTO(Guid Id, string Name, ChatMessageDTO? PreviewMessage)
{
  public static ChatDTO From(Chat v, AmazonS3Client? s3Client) =>
    new(
      Id: v.Id,
      Name: v.Customer.User.Phone,
      PreviewMessage: new Func<ChatMessageDTO?>(() =>
      {
        var lastMessage = v.Messages?.FirstOrDefault();
        return lastMessage is { } l ? ChatMessageDTO.From(l, s3Client) : null;
      })()
    );
};
