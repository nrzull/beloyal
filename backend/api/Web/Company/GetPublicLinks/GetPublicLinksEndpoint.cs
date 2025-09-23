using Beloyal.API.Companies;
using Beloyal.API.Messengers;
using Telegram.Bot;

namespace Beloyal.API.Web.Company.GetPublicLinks;

public class GetPublicLinksEndpoint(
  CompanyContext companyContext,
  ApplicationDbContext dbContext,
  TgContext tgContext
)
{
  public async Task<GetPublicLinksResponse> Handle(CompanyId companyId)
  {
    var company = await companyContext.GetCompany(companyId);

    await dbContext.Entry(company).Reference(v => v.TgBot).LoadAsync();
    await dbContext.Entry(company).Reference(v => v.MaxBot).LoadAsync();

    List<GetPublicLinksResponse.OkData> links = new() { };

    if (company.TgBot is { } tgbot)
    {
      var client = await tgContext.EnsureClient(companyId, tgbot);
      var me = await client.GetMe();
      links.Add(new("telegram", $"https://t.me/{me.Username}"));
    }

    return new GetPublicLinksResponse.Ok(links);
  }
}
