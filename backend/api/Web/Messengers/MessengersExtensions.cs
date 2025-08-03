using Beloyal.API.Web.Messengers.CreateTgBot;

namespace Beloyal.API.Web.Messengers;

public static class MessengersExtensions
{
  public static void UseMessengersEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<CreateTgBotEndpoint>();
  }
}
