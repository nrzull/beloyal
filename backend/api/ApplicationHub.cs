using System.Collections.Concurrent;
using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Microsoft.AspNetCore.SignalR;
using ConnectionId = string;

namespace Beloyal.API;

public class ApplicationHub : Hub
{
  static ConcurrentDictionary<CompanyId, List<ConnectionId>> Companies { get; } = new() { };

  public static List<string> GetCompanyConnections(CompanyId companyId) =>
    Companies.GetValueOrDefault(companyId, []);

  public override async Task OnConnectedAsync()
  {
    await HandleCompanyAccess();
    await base.OnConnectedAsync();
  }

  async Task HandleCompanyAccess()
  {
    var ctx = Context.GetHttpContext()!;
    var companyContext = ctx.RequestServices.GetRequiredService<CompanyContext>();

    if (ctx.Items["accessToken"] is AccessToken token && token.CompanyAccessId is Guid id)
    {
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
};
