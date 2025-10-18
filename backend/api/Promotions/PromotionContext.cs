using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Promotions.Variants;
using Beloyal.API.Shops;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Promotions;

public class PromotionContext(ApplicationDbContext dbContext)
{
  public async Task<List<Promotion>> GetPromotions(CompanyId companyId) =>
    await dbContext
      .Promotions.Where(v => v.CompanyId == companyId.Value)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<Promotion?> GetPromotion(CompanyId companyId, PromotionId promotionId) =>
    await dbContext.Promotions.FirstOrDefaultAsync(v =>
      v.Id == promotionId.Value && v.CompanyId == companyId.Value
    );

  public async Task<Promotion> CreatePromotion(Company company, string title, BaseVariant variant)
  {
    var promotion = new Promotion
    {
      Company = company,
      Title = title,
      Variant = variant,
    };
    dbContext.Add(promotion);
    await dbContext.SaveChangesAsync();

    return promotion;
  }

  public async Task UpdatePromotion(Promotion promotion, string title, BaseVariant variant)
  {
    dbContext.Update(promotion);
    promotion.Title = title;
    promotion.Variant = variant;
    await dbContext.SaveChangesAsync();
  }

  public async Task DeletePromotion(CompanyId companyId, PromotionId promotionId)
  {
    await dbContext
      .Promotions.Where(v => v.Id == promotionId.Value && v.CompanyId == companyId.Value)
      .ExecuteDeleteAsync();
  }

  public async Task<Promotion?> GetAppliablePromotion(
    Customer customer,
    Company company,
    Shop shop,
    List<Promotion> promotions
  )
  {
    var sorted = promotions.Slice(0, promotions.Count);
    sorted.Sort((v1, v2) => v1.Variant.CashbackPercent > v2.Variant.CashbackPercent ? -1 : 1);

    var tz = shop.TimeZone ?? company.TimeZone;

    Promotion? selected = null;
    foreach (var promotion in sorted)
    {
      bool success = promotion.Variant switch
      {
        ExactCountVariant v when await Check(CustomerId.From(customer), v) => true,
        DatePeriodVariant v when Check(tz, v) => true,
        TimePeriodVariant v when Check(tz, v) => true,
        DateTimePeriodVariant v when Check(tz, v) => true,
        _ => false,
      };

      if (success)
      {
        selected = promotion;
        break;
      }
    }

    return selected;
  }

  async Task<bool> Check(CustomerId customerId, ExactCountVariant variant)
  {
    var count = await dbContext
      .Bonuses.Where(v =>
        v.CustomerId == customerId.Value
        && v.Status != Bonuses.BonusStatus.Cancelled
        && (v.Type == Bonuses.BonusType.AddBonus || v.Type == Bonuses.BonusType.SubtractBonus)
      )
      .CountAsync();

    return (variant.Count - 1) == count;
  }

  bool Check(TimeZoneInfo tz, DatePeriodVariant variant)
  {
    var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz);
    var nowDateOnly = new DateOnly(now.Year, now.Month, now.Day);
    return variant.From <= nowDateOnly && variant.To >= nowDateOnly;
  }

  bool Check(TimeZoneInfo tz, TimePeriodVariant variant)
  {
    var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz);

    var from = new DateTimeOffset(
      now.Date.Year,
      now.Date.Month,
      now.Date.Day,
      variant.From.Hour,
      variant.From.Minute,
      variant.From.Second,
      variant.From.Millisecond,
      now.Offset
    );

    var to = new DateTimeOffset(
      now.Date.Year,
      now.Date.Month,
      now.Date.Day,
      variant.To.Hour,
      variant.To.Minute,
      variant.To.Second,
      variant.To.Millisecond,
      now.Offset
    );

    return from <= now && to >= now;
  }

  bool Check(TimeZoneInfo tz, DateTimePeriodVariant variant)
  {
    var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz);

    var from = new DateTimeOffset(
      variant.From.Year,
      variant.From.Month,
      variant.From.Day,
      variant.From.Hour,
      variant.From.Minute,
      variant.From.Second,
      variant.From.Millisecond,
      now.Offset
    );

    var to = new DateTimeOffset(
      variant.To.Year,
      variant.To.Month,
      variant.To.Day,
      variant.To.Hour,
      variant.To.Minute,
      variant.To.Second,
      variant.To.Millisecond,
      now.Offset
    );

    return from <= now && to >= now;
  }
}
