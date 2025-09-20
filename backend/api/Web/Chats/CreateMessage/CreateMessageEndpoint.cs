using Beloyal.API.Chats;
using Beloyal.API.Companies;
using Beloyal.API.Messengers;
using Microsoft.AspNetCore.SignalR;
using Telegram.Bot;

namespace Beloyal.API.Web.Chats.CreateMessage;

public class CreateMessageEndpoint(
  ChatContext chatContext,
  ApplicationDbContext dbContext,
  TgContext tgContext,
  IHubContext<ApplicationHub> appHub
)
{
  public async Task<CreateMessageResponse> Handle(
    CompanyAccess companyAccess,
    ChatId chatId,
    CreateMessageRequest req
  )
  {
    await using var tc = await dbContext.MaybeTransactionAsync();

    var chat = await chatContext.GetChat(chatId, CompanyId.From(companyAccess));
    if (chat is null)
    {
      throw new Exception("invalid chat");
    }

    var msg = await chatContext.SendMessage(companyAccess, chat.Customer, req.Content);

    var customer = chat.Customer;

    await dbContext.Entry(customer).Reference(v => v.TgBot).LoadAsync();
    await dbContext.Entry(customer).Reference(v => v.MaxBot).LoadAsync();

    var client = customer.TgBot is { }
      ? await tgContext.EnsureClient(CompanyId.From(customer.CompanyId), customer.TgBot)
      : null;

    if (client is null)
    {
      throw new Exception("client not found");
    }

    await dbContext.Entry(customer).Reference(v => v.User).LoadAsync();
    if (customer.User.TgUserId is null)
    {
      throw new Exception("tg user id not found");
    }

    await client.SendMessage(customer.User.TgUserId, req.Content);

    await appHub
      .Clients.Clients(ApplicationHub.GetCompanyConnections(CompanyId.From(companyAccess)) ?? [])!
      .SendAsync("ChatMessage", ChatMessageDTO.From(msg));

    await tc.CommitAsync();

    return new CreateMessageResponse.Ok();
  }
}
