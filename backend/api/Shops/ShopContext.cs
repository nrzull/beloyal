using Beloyal.API.Companies;
using Beloyal.API.Promotions;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Shops;

public class ShopContext(ApplicationDbContext dbContext)
{
  public async Task<Shop?> GetShop(CompanyId companyId, ShopId shopId) =>
    await dbContext.Shops.FirstOrDefaultAsync(v =>
      v.Id == shopId.Value && v.CompanyId == companyId.Value
    );

  public async Task<List<Shop>> GetShops(CompanyId companyId) =>
    await dbContext
      .Shops.Where(v => v.CompanyId == companyId.Value)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<List<Shop>> GetShops(CompanyAccessId companyAccessId) =>
    await dbContext
      .ShopAccesses.Include(v => v.Shop)
      .Where(v => v.CompanyAccessId == companyAccessId.Value)
      .Select(v => v.Shop)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<Shop?> GetShop(ShopAccessId shopAccessId) =>
    (
      await dbContext
        .ShopAccesses.Include(v => v.Shop)
        .FirstOrDefaultAsync(v => v.Id == shopAccessId.Value)
    )?.Shop;

  public async Task<Shop> CreateShop(Company company, string name, TimeZoneInfo? tz)
  {
    var shop = new Shop
    {
      Company = company,
      Name = name,
      TimeZone = tz,
    };
    dbContext.Add(shop);
    await dbContext.SaveChangesAsync();

    return shop;
  }

  public async Task UpdateShop(Shop shop, string name, TimeZoneInfo? tz)
  {
    dbContext.Update(shop);
    shop.Name = name;
    shop.TimeZone = tz;
    await dbContext.SaveChangesAsync();
  }

  public async Task<ShopAccess> CreateAccess(Shop shop, CompanyAccess companyAccess)
  {
    if (companyAccess.CompanyId != shop.CompanyId)
    {
      throw new Exception("company id mismatch");
    }

    var access = new ShopAccess { Shop = shop, CompanyAccess = companyAccess };
    dbContext.Add(access);
    await dbContext.SaveChangesAsync();

    return access;
  }

  public async Task RemoveAccess(ShopAccess shopAccess)
  {
    dbContext.Remove(shopAccess);
    await dbContext.SaveChangesAsync();
  }

  public async Task<ShopAccess?> GetAccess(ShopId shopId, CompanyAccessId companyAccessId) =>
    await dbContext.ShopAccesses.FirstOrDefaultAsync(v =>
      v.ShopId == shopId.Value && v.CompanyAccessId == companyAccessId.Value
    );

  public async Task<ShopAccess?> GetAccess(ShopAccessId id) =>
    await dbContext.ShopAccesses.Include(v => v.Shop).FirstOrDefaultAsync(v => v.Id == id.Value);

  public async Task<List<ShopAccess>> GetAccesses(ShopId shopId) =>
    await dbContext
      .ShopAccesses.Include(v => v.CompanyAccess)
      .ThenInclude(v => v.User)
      .Where(v => v.ShopId == shopId.Value)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<List<ShopAccess>> GetAccesses(CompanyId companyId, ShopId shopId) =>
    await dbContext
      .ShopAccesses.Include(v => v.CompanyAccess)
      .ThenInclude(v => v.User)
      .Where(v => v.ShopId == shopId.Value && v.CompanyAccess.CompanyId == companyId.Value)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<List<ShopPromotion>> GetPromotions(CompanyId companyId, ShopId shopId) =>
    await dbContext
      .ShopPromotions.Include(v => v.Promotion)
      .Where(v => v.ShopId == shopId.Value && v.Promotion.CompanyId == companyId.Value)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<ShopPromotion?> GetPromotion(
    CompanyId companyId,
    ShopPromotionId shopPromotionId
  ) =>
    await dbContext
      .ShopPromotions.Include(v => v.Promotion)
      .FirstOrDefaultAsync(v =>
        v.Id == shopPromotionId.Value && v.Promotion.CompanyId == companyId.Value
      );

  public async Task<ShopPromotion> AddPromotion(Shop shop, Promotion promotion)
  {
    var shopPromotion = new ShopPromotion { Promotion = promotion, Shop = shop };

    dbContext.Add(shopPromotion);
    await dbContext.SaveChangesAsync();

    return shopPromotion;
  }

  public async Task RemovePromotion(ShopPromotion shopPromotion)
  {
    dbContext.Remove(shopPromotion);
    await dbContext.SaveChangesAsync();
  }
}
