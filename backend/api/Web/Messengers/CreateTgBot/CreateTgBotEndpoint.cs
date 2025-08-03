using Beloyal.API.Companies;
using Beloyal.API.Messengers;

namespace Beloyal.API.Web.Messengers.CreateTgBot;

public class CreateTgBotEndpoint(
  CompanyContext companyContext,
  TgContext tgContext,
  ApplicationDbContext dbContext
)
{
  public async Task<CreateTgBotResponse> Handle(CompanyAccess access, CreateTgBotRequest req)
  {
    var company = await companyContext.GetCompany(CompanyId.From(access));

    await using var tc = await dbContext.MaybeTransactionAsync();
    var bot = await tgContext.CreateBot(company, req.Token);
    var client = await tgContext.EnsureClient(CompanyId.From(access), bot);
    await tc.CommitAsync();

    return new CreateTgBotResponse.Ok(await TgBotDTO.From(bot, client));
  }
}
