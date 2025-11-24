using Beloyal.API.Countries;
using Beloyal.API.Types;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Users;

public class UserContext(ApplicationDbContext dbContext)
{
  public async Task<User> GetById(UserId id) =>
    await dbContext
      .Users.Include(v => v.AdminAccess)
      .ThenInclude(v => v!.Role)
      .SingleAsync(v => v.Id == id.Value);

  public async Task<User?> GetByPhone(Phone phone) =>
    await dbContext.Users.FirstOrDefaultAsync(v => v.Phone == phone.Value);

  public async Task<User?> GetByTgUserId(TgUserId id) =>
    await dbContext.Users.FirstOrDefaultAsync(v => v.TgUserId == id.Value);

  public async Task EnsureTgUserId(User user, TgUserId id)
  {
    if (user.TgUserId != id.Value)
    {
      dbContext.Update(user);
      user.TgUserId = id.Value;
      await dbContext.SaveChangesAsync();
    }
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

  public async Task VerifyUser(User user)
  {
    if (user.VerifiedAt is not null)
    {
      return;
    }

    dbContext.Update(user);
    user.VerifiedAt = DateTimeOffset.UtcNow;
    await dbContext.SaveChangesAsync();
  }

  public async Task UpdateUserInfo(
    User user,
    string? firstName,
    string? lastName,
    string? middleName
  )
  {
    dbContext.Update(user);

    user.FirstName = firstName;
    user.LastName = lastName;
    user.MiddleName = middleName;

    await dbContext.SaveChangesAsync();
  }
}
