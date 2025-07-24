using Beloyal.API.Admin;
using Beloyal.API.Users;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Seeds;

public static class SeedAdminUser
{
  public static void Run(DbContext dbContext)
  {
    var email = Environment.GetEnvironmentVariable("API_ADMIN_EMAIL");
    if (email is null)
    {
      throw new Exception("missing API_ADMIN_EMAIL");
    }

    if (dbContext.Set<User>().FirstOrDefault(v => v.Email == email) is { })
    {
      return;
    }

    var user = new User { Email = email };

    var role = new AdminRole { Name = "Admin", Permissions = AdminPermissions.GetAllPermissions() };

    var adminAccess = new AdminAccess { User = user, Role = role };

    dbContext.AddRange(user, role, adminAccess);
    dbContext.SaveChanges();
  }
}
