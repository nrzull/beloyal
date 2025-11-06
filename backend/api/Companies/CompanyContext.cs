using Beloyal.API.Countries;
using Beloyal.API.Shops;
using Beloyal.API.Types;
using Beloyal.API.Users;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Companies;

public class CompanyContext(ApplicationDbContext dbContext, ShopContext shopContext)
{
  public async Task<List<WhiteCompany>> GetWhiteCompanies() =>
    await dbContext.WhiteCompanies.Include(v => v.Company).ToListAsync();

  public async Task<WhiteCompany?> GetWhiteCompany(WhiteCompanyId id) =>
    await dbContext.WhiteCompanies.FirstOrDefaultAsync(v => v.Id == id.Value);

  public async Task<WhiteCompany?> GetWhiteCompany(CompanyId id) =>
    await dbContext.WhiteCompanies.FirstOrDefaultAsync(v => v.CompanyId == id.Value);

  public async Task<WhiteCompany> CreateWhiteCompany(Tin tin, DateTimeOffset expiredAt)
  {
    var whiteCompany = new WhiteCompany { Tin = tin.Value, ExpiredAt = expiredAt };

    dbContext.Add(whiteCompany);
    await dbContext.SaveChangesAsync();

    return whiteCompany;
  }

  public async Task UpdateWhiteCompany(WhiteCompany whiteCompany, DateTimeOffset expiredAt)
  {
    dbContext.Update(whiteCompany);
    whiteCompany.ExpiredAt = expiredAt;
    await dbContext.SaveChangesAsync();
  }

  public async Task RemoveWhiteCompany(WhiteCompanyId id)
  {
    await dbContext.WhiteCompanies.Where(v => v.Id == id.Value).ExecuteDeleteAsync();
  }

  public async Task<CompanyRole> CreateCompanyRole(
    Company company,
    string name,
    List<string> permissions
  )
  {
    if (!CompanyPermissions.IsValid(permissions))
    {
      throw new Exception("invalid permissions");
    }

    var role = new CompanyRole
    {
      Name = name,
      Permissions = permissions,
      Company = company,
    };

    dbContext.Add(role);
    await dbContext.SaveChangesAsync();

    return role;
  }

  public async Task UpdateCompanyRole(CompanyRole role, string name, List<string> permissions)
  {
    if (!CompanyPermissions.IsValid(permissions))
    {
      throw new Exception("invalid permissions");
    }

    dbContext.Update(role);

    role.Name = name;
    role.Permissions = permissions;

    await dbContext.SaveChangesAsync();
  }

  public async Task<Company> GetCompany(CompanyId id) =>
    await dbContext.Companies.SingleAsync(v => v.Id == id.Value);

  public async Task<(Company, CompanyRole, CompanyAccess)> CreateCompany(
    User user,
    Tin tin,
    string title,
    Percent cashbackPercent,
    Percent cashbackThreshold,
    Percent bonusMultiplier,
    TimeSpan bonusLifetime,
    TimeZoneInfo tz,
    TimeSpan levelLifetime,
    CountryCode countryCode,
    List<CountryLang> countryLangs
  )
  {
    var whiteCompany = await dbContext.WhiteCompanies.FirstOrDefaultAsync(v =>
      v.Tin == tin.Value && v.ExpiredAt > DateTimeOffset.UtcNow
    );
    if (whiteCompany is null)
    {
      throw new Exception("company is not white");
    }

    var company = new Company
    {
      Tin = tin.Value,
      Title = title,
      TimeZone = tz,
      CashbackPercent = cashbackPercent.Value,
      CashbackThreshold = cashbackThreshold.Value,
      BonusMultiplier = bonusMultiplier.Value,
      BonusLifetime = bonusLifetime,
      LevelLifetime = levelLifetime,
      Owner = user,
      CountryCode = countryCode,
      CountryLangs = countryLangs,
    };

    var role = new CompanyRole
    {
      Name = "Admin",
      Company = company,
      Permissions = CompanyPermissions.GetAllPermissions(),
    };

    var access = new CompanyAccess
    {
      User = user,
      Company = company,
      Role = role,
    };

    var shop = new Shop { Company = company, Name = "Мой магазин" };
    var shopAccess = new ShopAccess { CompanyAccess = access, Shop = shop };

    dbContext.Update(whiteCompany);
    whiteCompany.Company = company;

    dbContext.AddRange(company, role, access, shop, shopAccess);
    await dbContext.SaveChangesAsync();

    return (company, role, access);
  }

  public async Task BlockAccess(CompanyAccess companyAccess)
  {
    dbContext.Update(companyAccess);
    companyAccess.BlockedAt = DateTimeOffset.UtcNow;
    await dbContext.SaveChangesAsync();
  }

  public async Task UnblockAccess(CompanyAccess companyAccess)
  {
    dbContext.Update(companyAccess);
    companyAccess.BlockedAt = null;
    await dbContext.SaveChangesAsync();
  }

  public async Task<CompanyAccess> CreateCompanyAccess(Company company, User user, CompanyRole role)
  {
    if (role.CompanyId != company.Id)
    {
      throw new Exception("invalid role");
    }

    var access = new CompanyAccess
    {
      Company = company,
      Role = role,
      User = user,
    };

    dbContext.Add(access);

    var shops = await shopContext.GetShops(CompanyId.From(company));
    var shop = shops.FirstOrDefault();

    if (shop is { } && shops.Count == 1)
    {
      var shopAccess = new ShopAccess { CompanyAccess = access, Shop = shop };
      dbContext.Add(shopAccess);
    }

    await dbContext.SaveChangesAsync();

    return access;
  }

  public async Task UpdateCompanyBonusOptions(
    Company company,
    Percent cashbackPercent,
    Percent cashbackThreshold,
    Percent bonusMultiplier,
    TimeSpan bonusLifetime,
    CompanyBonusFrequency? bonusFrequency,
    TimeSpan? bonusScheduling,
    decimal? welcomeBonus,
    TimeSpan levelLifetime
  )
  {
    dbContext.Update(company);
    company.CashbackPercent = cashbackPercent.Value;
    company.CashbackThreshold = cashbackThreshold.Value;
    company.BonusMultiplier = bonusMultiplier.Value;
    company.BonusLifetime = bonusLifetime;
    company.BonusFrequency = bonusFrequency;
    company.BonusScheduling = bonusScheduling;
    company.WelcomeBonus = welcomeBonus;
    company.LevelLifetime = levelLifetime;

    await dbContext.SaveChangesAsync();
  }

  public async Task UpdateCompanyAccess(CompanyAccess access, CompanyRole role)
  {
    if (role.CompanyId != access.CompanyId)
    {
      throw new Exception("invalid role");
    }

    dbContext.Update(access);
    access.Role = role;

    await dbContext.SaveChangesAsync();
  }

  public async Task RemoveCompanyAccess(CompanyAccess access)
  {
    dbContext.Remove(access);
    await dbContext.SaveChangesAsync();
  }

  public async Task<List<CompanyAccess>> GetCompanyAccesses(CompanyId id) =>
    await dbContext
      .CompanyAccesses.Where(v => v.CompanyId == id.Value)
      .Include(v => v.Role)
      .Include(v => v.Company)
      .Include(v => v.User)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<List<CompanyAccess>> GetCompanyAccesses(UserId id) =>
    await dbContext
      .CompanyAccesses.Include(v => v.Company)
      .Include(v => v.Role)
      .Where(v => v.UserId == id.Value)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<List<CompanyRole>> GetCompanyRoles(CompanyId id) =>
    await dbContext
      .CompanyRoles.Where(v => v.CompanyId == id.Value)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<CompanyRole?> GetRole(CompanyId companyId, CompanyRoleId id) =>
    await dbContext.CompanyRoles.FirstOrDefaultAsync(v =>
      v.Id == id.Value && v.CompanyId == companyId.Value
    );

  public async Task<CompanyAccess?> GetAccess(UserId userId, CompanyAccessId id) =>
    await dbContext.CompanyAccesses.FirstOrDefaultAsync(v =>
      v.UserId == userId.Value && v.Id == id.Value
    );

  public async Task<CompanyAccess?> GetAccess(CompanyAccessId id) =>
    await dbContext
      .CompanyAccesses.Include(v => v.Company)
      .Include(v => v.User)
      .Include(v => v.Role)
      .FirstOrDefaultAsync(v => v.Id == id.Value);

  public async Task<CompanyAccess?> GetAccess(CompanyId companyId, CompanyAccessId id) =>
    await dbContext
      .CompanyAccesses.Include(v => v.Company)
      .Include(v => v.User)
      .Include(v => v.Role)
      .FirstOrDefaultAsync(v => v.Id == id.Value && v.CompanyId == companyId.Value);

  public async Task UpdateCompany(Company company, Tin tin, string title, TimeZoneInfo timeZone)
  {
    dbContext.Update(company);
    company.Tin = tin.Value;
    company.Title = title;
    company.TimeZone = timeZone;
    await dbContext.SaveChangesAsync();
  }
}
