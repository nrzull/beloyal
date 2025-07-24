using Beloyal.API.Types;
using Beloyal.API.Users;

namespace Beloyal.API.Companies;

public class CompanyContext(ApplicationDbContext dbContext)
{
  public async Task<(Company, CompanyRole, CompanyAccess)> CreateCompany(User user, Tin tin)
  {
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
