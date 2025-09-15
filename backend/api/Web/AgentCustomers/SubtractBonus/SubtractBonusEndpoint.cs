using Beloyal.API.Auth;
using Beloyal.API.Bonuses;
using Beloyal.API.Customers;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.AgentCustomers.SubtractBonus;

public class SubtractBonusEndpoint(
  CustomerContext customerContext,
  BonusContext bonusContext,
  AuthContext authContext
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
    await bonusContext.SubtractBonus(customer, agent, req.Income, decoded.IssuedAt);

    return new SubtractBonusResponse.Ok();
  }
}
