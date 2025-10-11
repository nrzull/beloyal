using Beloyal.API.Companies;
using Beloyal.API.Promotions.Variants;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Promotions;

public class PromotionContext(ApplicationDbContext dbContext)
{
  public async Task<List<Promotion>> GetPromotions(CompanyId companyId) =>
    await dbContext.Promotions.Where(v => v.CompanyId == companyId.Value).ToListAsync();

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
}
