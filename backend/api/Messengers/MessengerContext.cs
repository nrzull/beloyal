using Beloyal.API.Companies;

namespace Beloyal.API.Messengers;

public class MessengerContext(ApplicationDbContext dbContext)
{
  public async Task<TgBot> CreateTgBot(Company company, string token)
  {
    var tgBot = new TgBot { Company = company, Token = token };

    dbContext.Add(tgBot);
    await dbContext.SaveChangesAsync();

    return tgBot;
  }

  public async Task<MaxBot> CreateMaxBot(Company company, string token)
  {
    var maxBot = new MaxBot { Company = company, Token = token };

    dbContext.Add(maxBot);
    await dbContext.SaveChangesAsync();

    return maxBot;
  }
}
