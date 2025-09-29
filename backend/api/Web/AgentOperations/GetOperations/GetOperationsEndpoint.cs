using Beloyal.API.Bonuses;
using Beloyal.API.Companies;
using Beloyal.API.Shops;
using Beloyal.API.Web.Operations.GetOperations;

namespace Beloyal.API.Web.AgentOperations.GetOperations;

public class GetOperationsEndpoint(BonusContext bonusContext)
{
  public async Task<GetOperationsResponse> Handle(
    CompanyAccess companyAccess,
    ShopAccess shopAccess
  )
  {
    var bonuses = await bonusContext.GetBonuses(CompanyId.From(companyAccess));

    var data = bonuses
      .Where(v => v.ShopId == shopAccess.ShopId)
      .Select(v => new GetOperationsResponse.OkData(
        Bonus: BonusDTO.From(v),
        Shop: v.Shop is { } ? ShopDTO.From(v.Shop) : null,
        Agent: v.Agent is { } ? CompanyAccessDTO.From(v.Agent) : null
      ))
      .ToList();

    return new GetOperationsResponse.Ok(data);
  }
}
