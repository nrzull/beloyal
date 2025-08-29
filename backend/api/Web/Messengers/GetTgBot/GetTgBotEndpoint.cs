using Beloyal.API.Companies;
using Beloyal.API.Messengers;

namespace Beloyal.API.Web.Messengers.GetTgBot;

public class GetTgBotEndpoint(TgContext tgContext)
{
  public async Task<GetTgBotResponse> Handle(CompanyAccess companyAccess)
  {
    var bot = await tgContext.GetBot(CompanyId.From(companyAccess));
    if (bot is null)
    {
      return new GetTgBotResponse.NotFound();
    }

    var client = await tgContext.EnsureClient(CompanyId.From(companyAccess), bot);

    return new GetTgBotResponse.Ok(await TgBotDTO.From(bot, client));
  }
}
