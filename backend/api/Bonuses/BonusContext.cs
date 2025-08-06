using Beloyal.API.Customers;
using Beloyal.API.Shops;

namespace Beloyal.API.Bonuses;

public class BonusContext(ApplicationDbContext dbContext)
{
  public async Task<BonusRecord> AddBonus(Customer customer, ShopAccess agent, decimal income)
  {
    if (income <= 0)
    {
      throw new Exception("invalid income");
    }

    await dbContext.Entry(agent).Reference(v => v.Shop).LoadAsync();
    await dbContext.Entry(agent.Shop).Reference(v => v.Company).LoadAsync();
    await dbContext.Entry(agent).Reference(v => v.CompanyAccess).LoadAsync();

    var cashbackPercent = agent.Shop.Company.CashbackPercent;
    var cashbackThreshold = agent.Shop.Company.CashbackThreshold;
    var appliedBonus = income / 100 * cashbackPercent;
    var finalBonus = customer.Bonus + appliedBonus;

    var bonusRecord = new BonusRecord
    {
      Type = BonusRecordType.AddBonus,
      Status = BonusRecordStatus.Done,
      Customer = customer,
      Agent = agent.CompanyAccess,
      Shop = agent.Shop,
      Income = income,
      Bonus = appliedBonus,
      CashbackPercent = cashbackPercent,
      CashbackThreshold = cashbackThreshold,
    };

    dbContext.Add(bonusRecord);
    dbContext.Update(customer);

    customer.Bonus = finalBonus;

    await dbContext.SaveChangesAsync();

    return bonusRecord;
  }
}
