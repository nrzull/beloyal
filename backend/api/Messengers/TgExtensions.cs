using Beloyal.API.Messengers.EnsureTgUser;
using Beloyal.API.Messengers.OnTgMessage;

namespace Beloyal.API.Messengers;

public static class TgExtensions
{
  public static void UseTgBotHandlers(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<EnsureTgUserHandler>();
    builder.Services.AddScoped<OnTgMessageHandler>();
  }
}
