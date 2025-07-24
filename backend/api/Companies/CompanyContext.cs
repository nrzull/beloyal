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
    var role = new CompanyRole { Company = company, Name = "Admin" };
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
}
