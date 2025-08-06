using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Messengers;
using Beloyal.API.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Beloyal.API.Test.Customers;

public static class CustomerFixture
{
  public static async Task<(Customer, TgBot)> Customer(
    AsyncServiceScope scope,
    Company company,
    string? phone = null,
    TgBot? tgBot = null
  )
  {
    phone ??= "+71234561913";
    var customerContext = scope.Get<CustomerContext>();
    var tgContext = scope.Get<TgContext>();

    var bot = tgBot ?? await tgContext.CreateBot(company, "qweqweqwe");
    var customer = await customerContext.EnsureCustomer(bot, new(phone));

    return (customer, bot);
  }
}
