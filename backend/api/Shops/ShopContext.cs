using Beloyal.API.Companies;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Shops;

public class ShopContext(ApplicationDbContext dbContext)
{
  public async Task<Shop?> GetShop(Company company, Guid id) =>
    await dbContext.Shops.FirstOrDefaultAsync(v => v.Id == id && v.CompanyId == company.Id);

  public async Task<Shop> CreateShop(Company company, string name)
  {
    var shop = new Shop { Company = company, Name = name };
    dbContext.Add(shop);
    await dbContext.SaveChangesAsync();

    return shop;
  }

  public async Task UpdateShop(Shop shop, string name)
  {
    dbContext.Update(shop);
    shop.Name = name;
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

  public async Task<List<ShopAccess>> GetAccesses(Shop shop) =>
    await dbContext
      .ShopAccesses.Include(v => v.CompanyAccess)
      .ThenInclude(v => v.User)
      .Where(v => v.ShopId == shop.Id)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();
}
