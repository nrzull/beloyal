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
          var msg = await chatContext.CreateMessage(
            chat.Company,
            customer,
            "Важное уведомление: завтра сгорят все бонусы."
          );

          await chatContext.SendMessage(msg);
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
