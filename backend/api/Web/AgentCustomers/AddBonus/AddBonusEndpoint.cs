using Beloyal.API.Auth;
using Beloyal.API.Bonuses;
using Beloyal.API.Chats;
using Beloyal.API.Customers;
using Beloyal.API.Shops;
using Microsoft.AspNetCore.SignalR;

namespace Beloyal.API.Web.AgentCustomers.AddBonus;

public class AddBonusEndpoint(
  BonusContext bonusContext,
  CustomerContext customerContext,
  AuthContext authContext,
  IHubContext<ApplicationHub> appHub,
  ILogger<AddBonusEndpoint> logger,
  ChatContext chatContext
)
{
  public async Task<AddBonusResponse> Handle(
    ShopAccess agent,
    Shop shop,
    string publicToken,
    AddBonusRequest req
  )
  {
    using var _ = logger.BeginScope("AddBonusEndpoint.Handle");

    var customer = await customerContext.GetCustomerByPublicToken(
      CompanyId.From(shop),
      publicToken
    );
    if (customer is null)
    {
      throw new Exception("invalid publicToken");
    }

    var decoded = await authContext.DecodeCustomerPublicAccessToken(publicToken);
    var bonus = await bonusContext.AddBonus(customer, agent, req.Income, decoded.IssuedAt);

    try
    {
      await chatContext.SendVirtualMessage(
        customer,
        bonus.Amount > 0 ? "Бонусы успешно начислены" : "Операция успешно выполнена"
      );
    }
    catch (Exception e)
    {
      logger.LogError(e, e.Message);
    }

    var conn = ApplicationHub.GetCustomerConnection(CustomerId.From(decoded.CustomerId))!;
    var wsClient = appHub.Clients.Client(conn);
    if (wsClient is { } client)
    {
      await client.SendAsync("Refresh");
    }

    return new AddBonusResponse.Ok(BonusDTO.From(bonus));
  }
}
