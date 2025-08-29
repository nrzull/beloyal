using Beloyal.API.Web.Messengers.CreateTgBot;
using Beloyal.API.Web.Messengers.GetTgBot;

namespace Beloyal.API.Web.Messengers;

public static class MessengersExtensions
{
  public static void UseMessengersEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<CreateTgBotEndpoint>();
    builder.Services.AddScoped<GetTgBotEndpoint>();
  }
}
