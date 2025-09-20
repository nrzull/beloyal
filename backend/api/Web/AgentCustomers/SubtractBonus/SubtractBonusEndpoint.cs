using Beloyal.API.Auth;
using Beloyal.API.Bonuses;
using Beloyal.API.Customers;
using Beloyal.API.Shops;
using Microsoft.AspNetCore.SignalR;

namespace Beloyal.API.Web.AgentCustomers.SubtractBonus;

public class SubtractBonusEndpoint(
  CustomerContext customerContext,
  BonusContext bonusContext,
  AuthContext authContext,
  IHubContext<ApplicationHub> appHub
)
{
  public async Task<SubtractBonusResponse> Handle(
    ShopAccess agent,
    Shop shop,
    string publicToken,
    SubtractBonusRequest req
  )
  {
    var customer = await customerContext.GetCustomerByPublicToken(
      CompanyId.From(shop),
      publicToken
    );
    if (customer is null)
    {
      throw new Exception("invalid publicToken");
    }

    var decoded = authContext.DecodeCustomerPublicAccessToken(publicToken);
    var bonus = await bonusContext.SubtractBonus(customer, agent, req.Income, decoded.IssuedAt);

    var conn = ApplicationHub.GetCustomerConnection(CustomerId.From(decoded.CustomerId))!;
    var wsClient = appHub.Clients.Client(conn);
    if (wsClient is { } client)
    {
      await client.SendAsync("Refresh");
    }

    return new SubtractBonusResponse.Ok(BonusDTO.From(bonus));
  }
}
