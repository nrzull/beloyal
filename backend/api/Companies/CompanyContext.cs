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

  public async Task<Company> GetCompany(CompanyAccess access) =>
    await dbContext.Companies.SingleAsync(v => v.Id == access.CompanyId);

  public async Task<Company> GetCompany(Guid id) =>
    await dbContext.Companies.SingleAsync(v => v.Id == id);

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
    await dbContext.SaveChangesAsync();

    return access;
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

  public async Task<List<CompanyAccess>> GetCompanyAccesses(Company company) =>
    await dbContext
      .CompanyAccesses.Where(v => v.CompanyId == company.Id)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<List<CompanyAccess>> GetCompanyAccesses(User user) =>
    await dbContext
      .CompanyAccesses.Include(v => v.Company)
      .Include(v => v.Role)
      .Where(v => v.UserId == user.Id)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<CompanyAccessInvitation> CreateCompanyAccessInvitation(
    Company company,
    CompanyRole role,
    Email email
  )
  {
    if (role.CompanyId != company.Id)
    {
      throw new Exception("invalid role");
    }

    if (
      await dbContext
        .CompanyAccesses.Include(v => v.User)
        .FirstOrDefaultAsync(v => v.User.Email == email.Value && v.CompanyId == company.Id) is
      { }
    )
    {
      throw new Exception("company access already exists");
    }

    if (
      await dbContext
        .CompanyAccessInvitations.Include(v => v.Company)
        .Include(v => v.Role)
        .FirstOrDefaultAsync(v =>
          v.Email == email.Value
          && v.Status == CompanyAccessInvitationStatus.Active
          && v.CompanyId == company.Id
        ) is
      { } existingInvitation
    )
    {
      return existingInvitation;
    }

    var invitation = new CompanyAccessInvitation
    {
      Company = company,
      Role = role,
      Email = email.Value,
    };

    dbContext.Add(invitation);
    await dbContext.SaveChangesAsync();

    return invitation;
  }

  public async Task UpdateCompanyAccessInvitation(
    CompanyAccessInvitation invitation,
    CompanyAccessInvitationStatus status
  )
  {
    dbContext.Update(invitation);
    invitation.Status = status;
    invitation.UpdatedAt = DateTimeOffset.UtcNow;
    await dbContext.SaveChangesAsync();
  }

  public async Task<CompanyAccessInvitation?> GetCompanyAccessInvitation(
    Company company,
    Email email
  ) =>
    await dbContext.CompanyAccessInvitations.SingleOrDefaultAsync(v =>
      v.CompanyId == company.Id && v.Email == email.Value
    );

  public async Task<List<CompanyAccessInvitation>> GetCompanyAccessInvitations(Company company) =>
    await dbContext
      .CompanyAccessInvitations.Where(v => v.CompanyId == company.Id)
      .OrderByDescending(v => v.UpdatedAt)
      .ToListAsync();

  public async Task<List<CompanyAccessInvitation>> GetCompanyAccessInvitations(Email email) =>
    await dbContext
      .CompanyAccessInvitations.Where(v => v.Email == email.Value)
      .OrderByDescending(v => v.UpdatedAt)
      .ToListAsync();

  public async Task<List<CompanyRole>> GetCompanyRoles(Company company) =>
    await dbContext
      .CompanyRoles.Where(v => v.CompanyId == company.Id)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<CompanyRole?> GetRole(Company company, Guid id) =>
    await dbContext.CompanyRoles.FirstOrDefaultAsync(v => v.Id == id && v.CompanyId == company.Id);

  public async Task<CompanyAccess?> GetAccess(User user, Guid id) =>
    await dbContext.CompanyAccesses.FirstOrDefaultAsync(v => v.UserId == user.Id && v.Id == id);

  public async Task<CompanyAccess?> GetAccess(Guid id) =>
    await dbContext.CompanyAccesses.Include(v => v.Company).FirstOrDefaultAsync(v => v.Id == id);
}
