using Beloyal.API.Companies;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Beloyal.API.Messengers;

public class TgContext(ApplicationDbContext dbContext) : IMessengerContext<TgBot, TelegramBotClient>
{
  static Dictionary<CompanyId, TelegramBotClient> Pool { get; set; } = new() { };

  public async Task<TgBot> CreateBot(Company company, string token)
  {
    var tgBot = new TgBot { Company = company, Token = token };

    dbContext.Add(tgBot);
    await dbContext.SaveChangesAsync();

    return tgBot;
  }

  public void DisconnectClient(CompanyId companyId)
  {
    if (Pool.TryGetValue(companyId, out var existing))
    {
      Pool.Remove(companyId);
      ((IDisposable)existing).Dispose();
    }
  }

  public async Task<TelegramBotClient> EnsureClient(CompanyId companyId, TgBot tgBot)
  {
    if (Pool.TryGetValue(companyId, out var existing))
    {
      return existing;
    }

    var client = new TelegramBotClient(tgBot.Token);
    await client.DropPendingUpdates();

    client.OnError += async (e, _) => await OnClientError(companyId, e);

    Pool.Add(companyId, client);

    return client;
  }

  async Task OnClientError(CompanyId companyId, Exception e)
  {
    var msg = e.ToString();
    await Console.Error.WriteLineAsync(msg);

    if (msg.Contains("409"))
    {
      DisconnectClient(companyId);
    }
  }

  public async Task PrepareClients()
  {
    var bots = await dbContext.TgBots.ToListAsync();

    await Parallel.ForEachAsync(
      bots,
      async (v, cts) =>
      {
        cts.ThrowIfCancellationRequested();

        try
        {
          await EnsureClient(CompanyId.From(v.Id), v);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
      }
    );
  }
}
