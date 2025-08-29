using Beloyal.API.Companies;
using Beloyal.API.Messengers;
using Beloyal.API.Types;
using Beloyal.API.Users;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Customers;

public class CustomerContext(
  ApplicationDbContext dbContext,
  UserContext userContext,
  CompanyContext companyContext
)
{
  public async Task<List<Customer>> GetCustomers(CompanyId companyId) =>
    await dbContext.Customers.Where(v => v.CompanyId == companyId.Value).ToListAsync();

  public async Task<Customer?> GetCustomer(CustomerId customerId) =>
    await dbContext.Customers.FirstOrDefaultAsync(v => v.Id == customerId.Value);

  public async Task<Customer?> GetCustomer(CompanyId companyId, UserId userId) =>
    await dbContext
      .Customers.Include(v => v.User)
      .FirstOrDefaultAsync(v => v.CompanyId == companyId.Value && v.UserId == userId.Value);

  public async Task<Customer> EnsureCustomer(TgBot tgBot, Phone phone)
  {
    var existing = await dbContext
      .Customers.Include(v => v.User)
      .Where(v => v.User.Phone == phone.Value && v.CompanyId == tgBot.CompanyId)
      .FirstOrDefaultAsync();

    if (existing is { })
    {
      if (existing.TgBot is null)
      {
        dbContext.Update(existing);
        existing.TgBot = tgBot;
        await dbContext.SaveChangesAsync();
      }

      return existing;
    }

    var user = await userContext.EnsureUser(phone);
    var customer = await CreateCustomer(user, null, tgBot);

    return customer;
  }

  public async Task<Customer> EnsureCustomer(MaxBot maxBot, Phone phone)
  {
    var existing = await dbContext
      .Customers.Include(v => v.User)
      .Where(v => v.User.Phone == phone.Value && v.CompanyId == maxBot.CompanyId)
      .FirstOrDefaultAsync();

    if (existing is { })
    {
      if (existing.MaxBot is null)
      {
        dbContext.Update(existing);
        existing.MaxBot = maxBot;
        await dbContext.SaveChangesAsync();
      }

      return existing;
    }

    var user = await userContext.EnsureUser(phone);
    var customer = await CreateCustomer(user, maxBot);

    return customer;
  }

  async Task<Customer> CreateCustomer(User user, MaxBot? maxBot = null, TgBot? tgBot = null)
  {
    var companyId = maxBot?.CompanyId ?? tgBot!.CompanyId;
    var customer = new Customer
    {
      User = user,
      Company = await companyContext.GetCompany(CompanyId.From(companyId)),
      MaxBot = maxBot,
      TgBot = tgBot,
    };

    dbContext.Add(customer);
    await dbContext.SaveChangesAsync();

    return customer;
  }
}
