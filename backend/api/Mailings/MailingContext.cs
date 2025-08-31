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
  public async Task<List<Mailing>> GetMailings(CompanyId companyId) =>
    await dbContext
      .Mailings.Where(v => v.CompanyId == companyId.Value)
      .Include(v => v.Units)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<Mailing> CreateMailing(
    Company company,
    string content,
    bool withShops,
    List<ShopId> shopIds
  )
  {
    var mailing = new Mailing { Company = company, Content = content };

    List<CustomerId> customerIds = [];

    if (withShops && shopIds.Count > 0)
    {
      foreach (var shopId in shopIds)
      {
        var bonuses = (
          await dbContext.BonusRecords.Where(v => v.ShopId == shopId.Value).ToListAsync()
        ).DistinctBy(v => v.CustomerId);

        customerIds = customerIds
          .Concat(bonuses.Select(vv => CustomerId.From(vv.CustomerId)))
          .ToList();
      }
    }

    customerIds = customerIds.DistinctBy(v => v.Value).ToList();

    var customers = withShops
      ? await customerContext.GetCustomers(CompanyId.From(company), customerIds)
      : await customerContext.GetCustomers(CompanyId.From(company));

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

    await client.SendMessage(customer.User.TgUserId, unit.Mailing.Content);

    dbContext.Update(unit);
    unit.Status = MailingUnitStatus.Done;

    await dbContext.SaveChangesAsync();
  }
}
