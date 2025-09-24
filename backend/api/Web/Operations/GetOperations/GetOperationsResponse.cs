using Beloyal.API.Bonuses;
using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Operations.GetOperations;

public abstract record GetOperationsResponse
{
  public record Ok(List<OkData> Data) : GetOperationsResponse;

  public record OkData(BonusDTO Bonus, ShopDTO? Shop, CompanyAccessDTO? Agent);
}
