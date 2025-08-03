using Vogen;

namespace Beloyal.API.Messengers;

[ValueObject<Guid>]
public partial struct TgBotId
{
  public static TgBotId From(TgBot v) => From(v.Id);
};
