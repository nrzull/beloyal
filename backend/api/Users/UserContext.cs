using Beloyal.API.Types;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Users;

public class UserContext(ApplicationDbContext dbContext)
{
  public async Task<User> GetById(UserId id) =>
    await dbContext.Users.SingleAsync(v => v.Id == id.Value);

  public async Task<User> EnsureUser(Email email)
  {
    var user = await dbContext.Users.Where(v => v.Email == email.Value).FirstOrDefaultAsync();

    if (user is { })
    {
      return user;
    }

    user = new User() { Email = email.Value };

    dbContext.Add(user);
    await dbContext.SaveChangesAsync();

    return user;
  }

  public async Task<User> EnsureUser(Phone phone)
  {
    var user = await dbContext.Users.Where(v => v.Phone == phone.Value).FirstOrDefaultAsync();

    if (user is { })
    {
      return user;
    }

    user = new User() { Phone = phone.Value };

    dbContext.Add(user);
    await dbContext.SaveChangesAsync();

    return user;
  }
}
