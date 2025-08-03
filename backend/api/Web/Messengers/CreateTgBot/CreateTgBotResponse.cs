using Beloyal.API.Messengers;

namespace Beloyal.API.Web.Messengers.CreateTgBot;

public abstract record CreateTgBotResponse
{
  public record Ok(TgBotDTO Data) : CreateTgBotResponse;
}
