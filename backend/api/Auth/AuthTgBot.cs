using Beloyal.API.Users;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Beloyal.API.Auth;

public class AuthTgBot(IServiceScopeFactory sf, ILogger<AuthTgBot> logger)
{
  TelegramBotClient? Client { get; set; }
  string? Username { get; set; }

  public abstract record SendMessageResult
  {
    public record UserNotFound : SendMessageResult;

    public record Ok : SendMessageResult;
  }

  public string GetUsername() => Username is { } ? Username : throw new Exception("no username");

  public async Task<SendMessageResult> SendCode(string phone, string code)
  {
    await using var scope = sf.CreateAsyncScope();
    var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();

    try
    {
      var user = await userContext.GetByPhone(new(phone));

      if (user is null || user.TgUserId is null)
      {
        return new SendMessageResult.UserNotFound();
      }

      await Client!.GetChat(user.TgUserId);
      await Client!.SendMessage(user.TgUserId, $"Код для входа в систему: {code}");
      return new SendMessageResult.Ok();
    }
    catch (Exception e)
    {
      logger.LogError(e, e.Message);
      return new SendMessageResult.UserNotFound();
    }
  }

  public async Task Setup(string token)
  {
    Client = new TelegramBotClient(token);
    await Client.DropPendingUpdates();

    var me = await Client.GetMe();
    Username = me.Username!;

    Client.OnMessage += async (msg, u) =>
    {
      try
      {
        if (msg.From is null || msg.From.IsBot)
        {
          return;
        }

        long uid = msg.From?.Id ?? default;
        if (uid is 0)
        {
          return;
        }

        await using var scope = sf.CreateAsyncScope();

        var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
        var user = await userContext.GetByTgUserId(TgUserId.From(uid));

        if (user is null && msg.Contact is null)
        {
          await RequestPhoneNumber(Client, msg.Chat.Id);
          return;
        }

        if (user is null && msg.Contact is { PhoneNumber: { } phoneNumber })
        {
          user = await userContext.EnsureUser(new(phoneNumber));
        }

        if (user is { })
        {
          await userContext.EnsureTgUserId(user, TgUserId.From(uid));
        }

        if (msg.Contact is { PhoneNumber: { } })
        {
          await Client.SendMessage(
            chatId: uid,
            text: "Регистрация прошла успешно. Теперь вы сможете получать код подтверждения для входа в систему.",
            parseMode: ParseMode.Html
          );
        }
      }
      catch (Exception error)
      {
        Console.WriteLine(error);
      }
    };
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
