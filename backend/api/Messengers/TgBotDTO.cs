using Telegram.Bot;

namespace Beloyal.API.Messengers;

public record TgBotDTO(
  Guid Id,
  string Name,
  string Username,
  string? Description,
  string? ShortDescription,
  DateTimeOffset CreatedAt,
  string? Photo
)
{
  public static async Task<TgBotDTO> From(TgBot v, TelegramBotClient client)
  {
    var me = await client.GetMe();
    var name = await client.GetMyName();
    var descr = await client.GetMyDescription();
    var shortDescr = await client.GetMyShortDescription();
    var photos = await client.GetUserProfilePhotos(me.Id);
    var photoInfo = photos.Photos?.FirstOrDefault()?.FirstOrDefault();

    string? photo = default;
    if (photoInfo is { } pi)
    {
      using var ms = new MemoryStream();
      var file = await client.GetFile(pi.FileId);
      await client.DownloadFile(file, ms);
      photo =
        ms.Length > 0 ? $@"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}" : null;
    }

    return new(
      Id: v.Id,
      Name: name.Name,
      CreatedAt: v.CreatedAt,
      Username: me.Username!,
      Description: descr?.Description,
      ShortDescription: shortDescr?.ShortDescription,
      Photo: photo
    );
  }
};
