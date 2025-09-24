using Beloyal.API.Bonuses;
using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Operations.GetOperations;

public class GetOperationsEndpoint(BonusContext bonusContext)
{
  public async Task<GetOperationsResponse> Handle(CompanyAccess companyAccess)
  {
    var bonuses = await bonusContext.GetBonuses(CompanyId.From(companyAccess));

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
