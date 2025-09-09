using Beloyal.API.Admin;
using Beloyal.API.Users;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Seeds;

public static class SeedAdminUser
{
  public static void Run(DbContext dbContext)
  {
    var phone = Environment.GetEnvironmentVariable("API_ADMIN_PHONE");
    if (phone is null)
    {
      throw new Exception("missing API_ADMIN_PHONE");
    }

    if (dbContext.Set<User>().FirstOrDefault(v => v.Phone == phone) is { })
    {
      return;
    }

    var user = new User { Phone = phone };

    var role = new AdminRole { Name = "Admin", Permissions = AdminPermissions.GetAllPermissions() };

    var adminAccess = new AdminAccess { User = user, Role = role };

    dbContext.AddRange(user, role, adminAccess);
    dbContext.SaveChanges();
  }
}
