using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Messengers;
using Beloyal.API.Shops;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Beloyal.API.Mailings;

public class MailingContext(
  ApplicationDbContext dbContext,
  CustomerContext customerContext,
  TgContext tgContext
)
{
  public async Task<Mailing> CreateMailing(Company company, string content, Shop? shop = null)
  {
    var mailing = new Mailing
    {
      Company = company,
      Shop = shop,
      Content = content,
    };

    var customers = await customerContext.GetCustomers(CompanyId.From(company));

    if (shop is { })
    {
      var bonuses = await dbContext
        .BonusRecords.Where(v => v.ShopId == shop.Id)
        .DistinctBy(v => v.CustomerId)
        .ToListAsync();

      customers = customers.Where(v => bonuses.Any(vv => vv.CustomerId == v.Id)).ToList();
    }

    mailing.Units = customers
      .Select(v => new MailingUnit { Mailing = mailing, Customer = v })
      .ToList();

    dbContext.Add(mailing);
    await dbContext.SaveChangesAsync();

    return mailing;
  }

  public async Task StartUnit(MailingUnit unit)
  {
    var customer = await customerContext.GetCustomer(CustomerId.From(unit.CustomerId));
    if (customer is null)
    {
      throw new Exception("invalid customer");
    }

    await dbContext.Entry(customer).Reference(v => v.TgBot).LoadAsync();
    await dbContext.Entry(customer).Reference(v => v.MaxBot).LoadAsync();

    var client = customer.TgBot is { }
      ? await tgContext.EnsureClient(CompanyId.From(customer.CompanyId), customer.TgBot)
      : null;

    if (client is null)
    {
      throw new Exception("client not found");
    }

    await dbContext.Entry(customer).Reference(v => v.User).LoadAsync();
    if (customer.User.TgUserId is null)
    {
      throw new Exception("tg user id not found");
    }

    await dbContext.Entry(unit).Reference(v => v.Mailing).LoadAsync();

    // TODO: send message
    // await client.SendMessage(customer.User.TgUserId, unit.Mailing.Content);

    dbContext.Update(unit);
    unit.Status = MailingUnitStatus.Done;

    await dbContext.SaveChangesAsync();
  }
}
