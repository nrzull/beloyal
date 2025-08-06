using Beloyal.API.Customers;
using Beloyal.API.Shops;
using Microsoft.EntityFrameworkCore;

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
    customer.Bonus += appliedBonus;

    await dbContext.SaveChangesAsync();

    return bonusRecord;
  }

  public async Task<BonusRecord> SubtractBonus(Customer customer, ShopAccess agent, decimal income)
  {
    if (income <= 0)
    {
      throw new Exception("invalid income");
    }

    if (customer.Bonus <= 0)
    {
      throw new Exception("invalid customer bonus");
    }

    await dbContext.Entry(agent).Reference(v => v.Shop).LoadAsync();
    await dbContext.Entry(agent.Shop).Reference(v => v.Company).LoadAsync();
    await dbContext.Entry(agent).Reference(v => v.CompanyAccess).LoadAsync();

    var cashbackPercent = agent.Shop.Company.CashbackPercent;
    var cashbackThreshold = agent.Shop.Company.CashbackThreshold;

    var bonusRecord = new BonusRecord
    {
      Type = BonusRecordType.SubtractBonus,
      Status = BonusRecordStatus.NeedConfirmation,
      Customer = customer,
      Shop = agent.Shop,
      Agent = agent.CompanyAccess,
      Income = income,
      CashbackPercent = cashbackPercent,
      CashbackThreshold = cashbackThreshold,
      ConfirmationCode = GenerateConfirmationCode(),
      ConfirmationCodeExpiredAt = DateTimeOffset.UtcNow.AddMinutes(5),
    };

    dbContext.Add(bonusRecord);
    await dbContext.SaveChangesAsync();

    return bonusRecord;
  }

  public async Task<BonusRecord> ConfirmSubtractBonus(BonusRecordId id, string confirmationCode)
  {
    var bonusRecord = await dbContext
      .BonusRecords.Include(v => v.Customer)
      .FirstOrDefaultAsync(v =>
        v.Id == id.Value
        && v.ConfirmationCode == confirmationCode
        && v.ConfirmationCodeExpiredAt > DateTimeOffset.UtcNow
        && v.Status == BonusRecordStatus.NeedConfirmation
      );

    if (bonusRecord is null)
    {
      throw new Exception("cannot confirm bonus subtraction");
    }

    if (bonusRecord.Customer.Bonus <= 0)
    {
      throw new Exception("invalid customer bonus");
    }

    dbContext.Update(bonusRecord);

    var incomeThreshold = bonusRecord.Income / 100 * bonusRecord.CashbackThreshold;

    var bonus =
      bonusRecord.Customer.Bonus > incomeThreshold ? incomeThreshold : bonusRecord.Customer.Bonus;

    bonusRecord.Status = BonusRecordStatus.Done;
    bonusRecord.Bonus = bonus;
    bonusRecord.Income -= bonus;
    bonusRecord.ConfirmationCode = null;
    bonusRecord.ConfirmationCodeExpiredAt = null;

    dbContext.Update(bonusRecord.Customer);
    bonusRecord.Customer.Bonus -= bonus;

    await dbContext.SaveChangesAsync();

    return bonusRecord;
  }

  string GenerateConfirmationCode() =>
    string.Join("", Enumerable.Range(1, 6).Select(_v => Random.Shared.NextInt64(1, 9)).ToList());
}
