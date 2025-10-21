using Beloyal.API.Chats;
using Beloyal.API.Messengers.EnsureTgUser;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Beloyal.API.Messengers.OnTgMessage;

public class OnTgMessageHandler(
  ChatContext chatContext,
  EnsureTgUserHandler ensureTgUserHandler,
  TgContext tgContext
)
{
  public async Task Handle(TelegramBotClient client, CompanyId companyId, Message msg, UpdateType _)
  {
    var bot = await tgContext.GetBot(companyId);
    if (bot is null)
    {
      return;
    }

    if (
      await ensureTgUserHandler.Handle(bot, client, companyId, msg)
      is not EnsureTgUserResponse.Ok ok
    )
    {
      return;
    }
    else if (msg.Text is not string { } text)
    {
      return;
    }
    else if (text.Contains("/start"))
    {
      await OnCustomerStart(ok);
    }
    else
    {
      await chatContext.EnsureConversation(ok.Customer);
      var newMessage = await chatContext.CreateMessage(ok.Customer, text);
      await chatContext.SendMessage(newMessage, [], toCustomer: false);
    }
  }

  async Task OnCustomerStart(EnsureTgUserResponse.Ok ok)
  {
    await ok.Client.SendMessage(
      ok.Customer.User.TgUserId!,
      $"{ok.Customer.CreatedAt} {ok.Customer.User.Phone}"
    );
  }
}
