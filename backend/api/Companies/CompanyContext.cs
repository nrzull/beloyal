using Beloyal.API.Types;
using Beloyal.API.Users;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Companies;

public class CompanyContext(ApplicationDbContext dbContext)
{
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

  public async Task RemoveWhiteCompany(Tin tin)
  {
    await dbContext.WhiteCompanies.Where(v => v.Tin == tin.Value).ExecuteDeleteAsync();
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

  public async Task<(Company, CompanyRole, CompanyAccess)> CreateCompany(User user, Tin tin)
  {
    var whiteCompany = await dbContext.WhiteCompanies.FirstOrDefaultAsync(v =>
      v.Tin == tin.Value && v.ExpiredAt > DateTimeOffset.UtcNow
    );
    if (whiteCompany is null)
    {
      throw new Exception("company is not white");
    }

    var company = new Company { Tin = tin.Value };

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

    dbContext.AddRange(company, role, access);
    await dbContext.SaveChangesAsync();

    return (company, role, access);
  }

  public async Task CreateCompanyAccess(Company company, User user, CompanyRole role)
  {
    throw new NotImplementedException();
  }

  public async Task UpdateCompanyAccess(Company company, User user, CompanyRole role)
  {
    throw new NotImplementedException();
  }

  public async Task RemoveCompanyAccess(Company company, User user)
  {
    throw new NotImplementedException();
  }
}
