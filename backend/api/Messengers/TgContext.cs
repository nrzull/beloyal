using Beloyal.API.Companies;
using Beloyal.API.Messengers.EnsureTgUser;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Beloyal.API.Messengers;

public class TgContext(ApplicationDbContext dbContext, IServiceScopeFactory sf)
  : IMessengerContext<TgBot, TelegramBotClient>
{
  static Dictionary<CompanyId, TelegramBotClient> Pool { get; set; } = new() { };

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
          await EnsureClient(CompanyId.From(v.CompanyId), v);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
      }
    );
  }

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

  public async Task<TgBot?> GetBot(CompanyId companyId) =>
    await dbContext.TgBots.FirstOrDefaultAsync(v => v.CompanyId == companyId.Value);

  public async Task<TelegramBotClient> EnsureClient(CompanyId companyId, TgBot tgBot)
  {
    if (Pool.TryGetValue(companyId, out var existing))
    {
      return existing;
    }

    var client = new TelegramBotClient(tgBot.Token);
    await client.DropPendingUpdates();

    client.OnError += async (e, _) => await OnError(companyId, e);
    client.OnMessage += async (msg, u) => await OnMessage(client, companyId, msg, u);

    Pool.Add(companyId, client);

    return client;
  }

  async Task OnError(CompanyId companyId, Exception e)
  {
    var msg = e.ToString();
    await Console.Error.WriteLineAsync(msg);

    if (msg.Contains("409"))
    {
      DisconnectClient(companyId);
    }
  }

  async Task OnMessage(TelegramBotClient client, CompanyId companyId, Message msg, UpdateType _)
  {
    try
    {
      await using var scope = sf.CreateAsyncScope();

      var ensureTgUserHandler = scope.ServiceProvider.GetRequiredService<EnsureTgUserHandler>();

      if (
        await ensureTgUserHandler.Handle(client, companyId, msg) is not EnsureTgUserResponse.Ok ok
      )
      {
        return;
      }

      if (
        (msg.Text is string { } && msg.Text.Contains("/start"))
        || msg.Contact is { PhoneNumber: { } }
      )
      {
        await OnCustomerStart(ok);
        return;
      }
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
    }
  }

  async Task OnCustomerStart(EnsureTgUserResponse.Ok ok)
  {
    await ok.Client.SendMessage(
      ok.Customer.User.TgUserId!,
      $"{ok.Customer.CreatedAt} {ok.Customer.User.Phone}"
    );
  }
}
