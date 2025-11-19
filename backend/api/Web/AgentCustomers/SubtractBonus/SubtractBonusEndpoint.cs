using Beloyal.API.Auth;
using Beloyal.API.Bonuses;
using Beloyal.API.Chats;
using Beloyal.API.Customers;
using Beloyal.API.Shops;
using Microsoft.AspNetCore.SignalR;

namespace Beloyal.API.Web.AgentCustomers.SubtractBonus;

public class SubtractBonusEndpoint(
  CustomerContext customerContext,
  BonusContext bonusContext,
  AuthContext authContext,
  IHubContext<ApplicationHub> appHub,
  ChatContext chatContext,
  ILogger<SubtractBonusEndpoint> logger
)
{
  public async Task<SubtractBonusResponse> Handle(
    ShopAccess agent,
    Shop shop,
    string publicToken,
    SubtractBonusRequest req
  )
  {
    using var _ = logger.BeginScope("SubtractBonusEndpoint");

    var customer = await customerContext.GetCustomerByPublicToken(
      CompanyId.From(shop),
      publicToken
    );
    if (customer is null)
    {
      throw new Exception("invalid publicToken");
    }

    var decoded = await authContext.DecodeCustomerPublicAccessToken(publicToken);
    var bonus = await bonusContext.SubtractBonus(customer, agent, req.Income, decoded.IssuedAt);

    var conn = ApplicationHub.GetCustomerConnection(CustomerId.From(decoded.CustomerId))!;
    var wsClient = appHub.Clients.Client(conn);

    try
    {
      await chatContext.SendVirtualMessage(
        customer,
        bonus.Amount > 0 ? "Бонусы успешно списаны" : "Операция успешно выполнена"
      );
    }
    catch (Exception e)
    {
      logger.LogError(e, e.Message);
    }

    if (wsClient is { } client)
    {
      await client.SendAsync("Refresh");
    }

    return new SubtractBonusResponse.Ok(BonusDTO.From(bonus));
  }
}
