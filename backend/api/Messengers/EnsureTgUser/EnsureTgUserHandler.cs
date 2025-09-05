using Beloyal.API.Customers;
using Beloyal.API.Types;
using Beloyal.API.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Beloyal.API.Messengers.EnsureTgUser;

public class EnsureTgUserHandler(CustomerContext customerContext, UserContext userContext)
{
  public async Task<EnsureTgUserResponse> Handle(
    TgBot tgBot,
    TelegramBotClient client,
    CompanyId companyId,
    Message msg
  )
  {
    if (msg.From is null || msg.From.IsBot)
    {
      return new EnsureTgUserResponse.InvalidMessage();
    }

    long uid = msg.From?.Id ?? default;
    if (uid is 0)
    {
      return new EnsureTgUserResponse.InvalidMessage();
    }

    var user = await userContext.GetByTgUserId(TgUserId.From(uid));

    var customer = user is { }
      ? await customerContext.GetCustomer(companyId, UserId.From(user))
      : null;

    if (user is null && msg.Contact is null)
    {
      await RequestPhoneNumber(client, msg.Chat.Id);
      return new EnsureTgUserResponse.RequestedPhoneNumber();
    }
    else if (user is null && msg.Contact is { PhoneNumber: { } phoneNumber })
    {
      customer = await customerContext.EnsureCustomer(tgBot, new Phone(phoneNumber));
      await userContext.EnsureTgUserId(customer.User, TgUserId.From(uid));
    }

    if (customer is null)
    {
      return new EnsureTgUserResponse.InvalidCustomer();
    }

    var url = Environment.GetEnvironmentVariable("CUSTOMER_WEB_PUBLIC_URL")! + "/telegram";

    var text = "Открыть кабинет";

    if (
      await client.GetChatMenuButton(chatId: uid) is MenuButtonWebApp btn
      && (btn.WebApp.Url != url || btn.Text != text)
    )
    {
      await client.SetChatMenuButton(
        chatId: uid,
        menuButton: new MenuButtonWebApp
        {
          Text = text,
          WebApp = new() { Url = url },
        }
      );
    }

    return new EnsureTgUserResponse.Ok(client, customer);
  }

  async Task RequestPhoneNumber(TelegramBotClient client, long chatUid)
  {
    var requestContactKeyboard = new ReplyKeyboardMarkup(
      new[] { new KeyboardButton("📱 Поделиться номером телефона") { RequestContact = true } }
    )
    {
      ResizeKeyboard = true,
      OneTimeKeyboard = true,
    };

    await client.SendMessage(
      chatUid,
      text: "Для завершения регистрации нажмите кнопку <b>'📱 Поделиться номером телефона'</b>. Нажимая кнопку, вы соглашаетесь с <b>персональной обработкой данных</b>.",
      replyMarkup: requestContactKeyboard,
      parseMode: ParseMode.Html
    );
  }
}
