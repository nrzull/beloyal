using System.Collections.Concurrent;
using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Microsoft.AspNetCore.SignalR;
using ConnectionId = string;

namespace Beloyal.API;

public class ApplicationHub : Hub
{
  static ConcurrentDictionary<CompanyId, List<ConnectionId>> Companies { get; } = new() { };

  static ConcurrentDictionary<CustomerId, ConnectionId> Customers { get; } = new() { };

  public static List<string> GetCompanyConnections(CompanyId companyId) =>
    Companies.GetValueOrDefault(companyId, []);

  public static string? GetCustomerConnection(CustomerId customerId) =>
    Customers.GetValueOrDefault(customerId);

  public override async Task OnConnectedAsync()
  {
    await HandleCompanyAccess();
    await HandleCustomerAccess();
    await base.OnConnectedAsync();
  }

  async Task HandleCompanyAccess()
  {
    var ctx = Context.GetHttpContext()!;

    if (
      ctx.Items.ContainsKey("accessToken")
      && ctx.Items["accessToken"] is AccessToken token
      && token.CompanyAccessId is Guid id
    )
    {
      var companyContext = ctx.RequestServices.GetRequiredService<CompanyContext>();

      var companyAccess = await companyContext.GetAccess(CompanyAccessId.From(id));
      if (companyAccess is null)
      {
        return;
      }

      Companies.TryGetValue(CompanyId.From(companyAccess), out var connectionIds);
      if (connectionIds is null)
      {
        Companies.TryAdd(CompanyId.From(companyAccess), []);
      }

      connectionIds!.Add(Context.ConnectionId);
    }
  }

  async Task HandleCustomerAccess()
  {
    var ctx = Context.GetHttpContext()!;
    if (
      ctx.Items.ContainsKey("customerAccessToken")
      && ctx.Items["customerAccessToken"] is CustomerAccessToken token
    )
    {
      var customerContext = ctx.RequestServices.GetRequiredService<CustomerContext>();

      var customer = await customerContext.GetCustomer(CustomerId.From(token.CustomerId));
      if (customer is null)
      {
        return;
      }

      Customers.AddOrUpdate(
        CustomerId.From(token.CustomerId),
        (_) => Context.ConnectionId,
        (_, _) => Context.ConnectionId
      );
    }
  }
};
