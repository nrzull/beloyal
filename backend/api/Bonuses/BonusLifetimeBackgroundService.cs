using Beloyal.API.Chats;
using Beloyal.API.Messengers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Beloyal.API.Bonuses;

public class BonusLifetimeBackgroundService(IServiceScopeFactory sf) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken ct)
  {
    _ = ReadNewBonuses(ct);
    await ProcessExistingBonuses(ct);
  }

  public async Task ProcessExistingBonuses(CancellationToken ct)
  {
    PeriodicTimer time = new(TimeSpan.FromMinutes(1));

    while (await time.WaitForNextTickAsync(ct))
    {
      await using var scope = sf.CreateAsyncScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
      var bonusContext = scope.ServiceProvider.GetRequiredService<BonusContext>();
      var chatContext = scope.ServiceProvider.GetRequiredService<ChatContext>();
      var tgContext = scope.ServiceProvider.GetRequiredService<TgContext>();
      var appHub = scope.ServiceProvider.GetRequiredService<IHubContext<ApplicationHub>>();

      var bonuses = await bonusContext.GetLatestBonuses();
      var now = DateTimeOffset.UtcNow;

      foreach (var bonus in bonuses)
      {
        var customer = bonus.Customer;

        if (customer.BonusAmount <= 0)
        {
          continue;
        }

        var deadline = bonus.CreatedAt.Add(customer.Company.BonusLifetime);

        var notificationDeadline = deadline.AddDays(-1);
        var chat = await chatContext.EnsureConversation(customer);

        var notification = await dbContext
          .ChatMessages.Where(v =>
            v.ChatId == chat.Id
            && v.CompanyId == chat.CompanyId
            && v.CreatedAt >= notificationDeadline
          )
          .FirstOrDefaultAsync();

        if (notification is null)
        {
          await dbContext.Entry(customer).Reference(v => v.TgBot).LoadAsync();
          await dbContext.Entry(customer).Reference(v => v.MaxBot).LoadAsync();
          await dbContext.Entry(customer).Reference(v => v.User).LoadAsync();

          var client = customer.TgBot is { }
            ? await tgContext.EnsureClient(CompanyId.From(customer.CompanyId), customer.TgBot)
            : null;

          if (client is { } c)
          {
            var content = "Важное уведомление: завтра сгорят все бонусы.";

            await c.SendMessage(customer.User.TgUserId!, content);
            var msg = await chatContext.CreateMessage(chat.Company, customer, content);

            await appHub
              .Clients.Clients(
                ApplicationHub.GetCompanyConnections(CompanyId.From(customer)) ?? []
              )!
              .SendAsync("ChatMessage", ChatMessageDTO.From(msg));
          }
        }

        if (deadline < now)
        {
          try
          {
            await bonusContext.BurnBonus(customer);
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
          }
        }
      }
    }
  }

  public async Task ReadNewBonuses(CancellationToken ct)
  {
    await foreach (var bonusId in BonusContext.NewBonusChannel.Reader.ReadAllAsync(ct))
    {
      Console.WriteLine($"new bonus: {bonusId}");
    }
  }
}
