using Beloyal.API.Messengers;

namespace Beloyal.API.Web.Messengers.GetTgBot;

public abstract record GetTgBotResponse
{
  public record NotFound : GetTgBotResponse;

  public record Ok(TgBotDTO Data) : GetTgBotResponse;
}
