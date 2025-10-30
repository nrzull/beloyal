using Amazon.S3;
using Amazon.S3.Model;

namespace Beloyal.API.Chats;

public record ChatMessageDTO(
  Guid Id,
  bool IsSystem,
  DateTimeOffset CreatedAt,
  DateTimeOffset? ReadAt,
  string? Content,
  Guid? CompanyId,
  Guid? AgentId,
  Guid? CustomerId,
  Guid ChatId,
  string Name,
  Guid? MailingUnitId,
  List<string> Attachments
)
{
  public static ChatMessageDTO From(ChatMessage v, AmazonS3Client? s3Client) =>
    new(
      Id: v.Id,
      IsSystem: v.MailingUnit is { } || v.CompanyId is { },
      CreatedAt: v.CreatedAt,
      ReadAt: v.ReadAt,
      AgentId: v.AgentId,
      CustomerId: v.CustomerId,
      ChatId: v.ChatId,
      CompanyId: v.CompanyId,
      Content: v.Content,
      Name: v.Agent is { } g ? g.User!.Phone!
        : v.Customer is { } c ? c.User!.Phone!
        : v.MailingUnitId is { } ? "Рассылка"
        : v.CompanyId is { } ? "Компания"
        : "Неизвестно",
      MailingUnitId: v.MailingUnitId,
      Attachments: s3Client is { }
        ? new Func<List<string>>(() =>
        {
          List<string> result = [];

          var bucketName = v.MailingUnitId is { }
            ? v.MailingUnit!.MailingId.ToString()
            : v.Id.ToString();

          var keys = v.MailingUnitId is { }
            ? v.MailingUnit!.Mailing.AttachmentKeys
            : v.AttachmentKeys;

          foreach (var key in keys ?? [])
          {
            var request = new GetPreSignedUrlRequest
            {
              BucketName = bucketName,
              Key = key,
              Verb = HttpVerb.GET,
              Expires = DateTime.UtcNow.AddMonths(1),
            };

            result.Add(s3Client.GetPreSignedURL(request));
          }

          return result;
        })()
        : []
    );
};
