using Beloyal.API.Bonuses;
using Beloyal.API.Customers;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.AgentCustomers.AddBonus;

public class AddBonusEndpoint(BonusContext bonusContext, CustomerContext customerContext)
{
  public async Task<AddBonusResponse> Handle(
    ShopAccess agent,
    Shop shop,
    string publicToken,
    AddBonusRequest req
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

    await bonusContext.AddBonus(customer, agent, req.Income);

    return new AddBonusResponse.Ok();
  }
}
