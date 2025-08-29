using Beloyal.API.Messengers.EnsureTgUser;

namespace Beloyal.API.Messengers;

public static class TgExtensions
{
  public static void UseTgBotHandlers(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<EnsureTgUserHandler>();
  }
}
