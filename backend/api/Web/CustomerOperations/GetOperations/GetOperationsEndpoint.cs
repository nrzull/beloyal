using Beloyal.API.Bonuses;
using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Shops;
using Beloyal.API.Web.Operations.GetOperations;

namespace Beloyal.API.Web.CustomerOperations.GetOperations;

public class GetOperationsEndpoint(BonusContext bonusContext)
{
  public async Task<GetOperationsResponse> Handle(Customer customer)
  {
    var bonuses = await bonusContext.GetBonuses(
      CompanyId.From(customer),
      CustomerId.From(customer)
    );

    var data = bonuses
      .Select(v => new GetOperationsResponse.OkData(
        Bonus: BonusDTO.From(v),
        Shop: v.Shop is { } ? ShopDTO.From(v.Shop) : null,
        Agent: v.Agent is { } ? CompanyAccessDTO.From(v.Agent) : null
      ))
      .ToList();

    return new GetOperationsResponse.Ok(data);
  }
}
