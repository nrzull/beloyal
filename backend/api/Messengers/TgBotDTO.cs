using Telegram.Bot;

namespace Beloyal.API.Messengers;

public record TgBotDTO(Guid Id, string Name, DateTimeOffset CreatedAt)
{
  public static async Task<TgBotDTO> From(TgBot v, TelegramBotClient client)
  {
    var me = await client.GetMe();
    return new(Id: v.Id, CreatedAt: v.CreatedAt, Name: me.Username!);
  }
};
