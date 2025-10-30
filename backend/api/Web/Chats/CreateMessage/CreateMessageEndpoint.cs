using Beloyal.API.Chats;
using Beloyal.API.Companies;

namespace Beloyal.API.Web.Chats.CreateMessage;

public class CreateMessageEndpoint(ChatContext chatContext, ApplicationDbContext dbContext)
{
  public async Task<CreateMessageResponse> Handle(
    CompanyAccess companyAccess,
    ChatId chatId,
    CreateMessageRequest req
  )
  {
    await using var tc = await dbContext.MaybeTransactionAsync();

    var chat = await chatContext.GetChat(CompanyId.From(companyAccess), chatId);
    if (chat is null)
    {
      throw new Exception("invalid chat");
    }

    var msg = await chatContext.CreateMessage(
      companyAccess,
      chat.Customer,
      req.Content,
      req.Attachments
    );
    await chatContext.SendMessage(
      msg,
      req.Attachments?.Select(v => new ChatFileStream(
          v.ContentType,
          v.FileName,
          v.OpenReadStream()
        ))
        .ToList()
    );

    await tc.CommitAsync();

    return new CreateMessageResponse.Ok();
  }
}
