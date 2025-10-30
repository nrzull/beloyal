using Amazon.S3;
using Beloyal.API.Chats;
using Beloyal.API.Companies;

namespace Beloyal.API.Web.Chats.GetChatMessages;

public class GetChatMessagesEndpoint(ChatContext chatContext, AmazonS3Client s3client)
{
  public async Task<GetChatMessagesResponse> Handle(
    CompanyAccess companyAccess,
    ChatId chatId,
    bool withSystem
  )
  {
    var messages = await chatContext.GetChatMessages(
      CompanyId.From(companyAccess),
      chatId,
      withSystem
    );

    return new GetChatMessagesResponse.Ok(
      messages.Select((v) => ChatMessageDTO.From(v, s3client)).ToList()
    );
  }
}
