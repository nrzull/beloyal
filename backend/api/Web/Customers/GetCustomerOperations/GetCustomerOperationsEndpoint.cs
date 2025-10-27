using Beloyal.API.Bonuses;
using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Shops;
using Beloyal.API.Web.Operations.GetOperations;

namespace Beloyal.API.Web.Customers.GetCustomerOperations;

public class GetCustomerOperationsEndpoint(BonusContext bonusContext)
{
  public async Task<GetOperationsResponse> Handle(
    CompanyAccess companyAccess,
    CustomerId customerId
  )
  {
    var bonuses = await bonusContext.GetBonuses(CompanyId.From(companyAccess), customerId);

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
