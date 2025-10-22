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
    else if (msg.Text == "/start")
    {
      await OnCustomerStart(ok);
    }
    else
    {
      var fileId =
        msg.Photo?.OrderByDescending(v => v.FileSize).FirstOrDefault()?.FileId
        ?? msg.Document?.FileId;

      var mimetype = msg.Photo?.OrderByDescending(v => v.FileSize).FirstOrDefault() is { }
        ? "image/jpeg"
        : msg.Document?.MimeType ?? "application/octet-stream";

      var msgText = msg.Text ?? msg.Caption ?? "";

      FileStream? fileStream = null;
      if (fileId is string { Length: > 0 } fid)
      {
        fileStream = new FileStream(path: Path.GetTempFileName(), mode: FileMode.OpenOrCreate);

        var file = await client.GetInfoAndDownloadFile(fileId, fileStream);
      }

      await chatContext.EnsureConversation(ok.Customer);
      var newMessage = await chatContext.CreateMessage(
        ok.Customer,
        msgText,
        fileStream is { } ? [new ChatFileStream(mimetype, fileStream)] : null
      );
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
