using Beloyal.API.Users;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Admin;

public class AdminContext(ApplicationDbContext dbContext)
{
  public async Task<AdminRole> CreateAdminRole(string name, List<string> permissions)
  {
    if (!AdminPermissions.IsValid(permissions))
    {
      throw new Exception("invalid permissions");
    }

    var role = new AdminRole { Name = name, Permissions = permissions };

    dbContext.Add(role);
    await dbContext.SaveChangesAsync();

    return role;
  }

  public async Task UpdateAdminRole(AdminRole role, string name, List<string> permissions)
  {
    if (!AdminPermissions.IsValid(permissions))
    {
      throw new Exception("invalid permissions");
    }

    dbContext.Update(role);

    role.Name = name;
    role.Permissions = permissions;

    await dbContext.SaveChangesAsync();
  }

  public async Task<AdminRole?> GetRole(Guid id) =>
    await dbContext.AdminRoles.FirstOrDefaultAsync(v => v.Id == id);

  public async Task<AdminAccess> CreateAdminAccess(User user, AdminRole role)
  {
    var adminAccess = new AdminAccess { User = user, Role = role };
    dbContext.Add(adminAccess);
    await dbContext.SaveChangesAsync();

    return adminAccess;
  }

  public async Task UpdateAdminAccess(AdminAccess adminAccess, AdminRole role)
  {
    dbContext.Update(adminAccess);
    adminAccess.Role = role;
    await dbContext.SaveChangesAsync();
  }

  public async Task RemoveAdminAccess(AdminAccess adminAccess)
  {
    dbContext.Remove(adminAccess);
    await dbContext.SaveChangesAsync();
  }

  public async Task<AdminAccess?> GetAccess(User user) =>
    await dbContext
      .AdminAccesses.Include(v => v.Role)
      .FirstOrDefaultAsync(v => v.UserId == user.Id);
}
