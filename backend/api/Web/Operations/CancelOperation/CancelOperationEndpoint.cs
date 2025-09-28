using Beloyal.API.Bonuses;
using Beloyal.API.Companies;

namespace Beloyal.API.Web.Operations.CancelOperation;

public class CancelOperationEndpoint(BonusContext bonusContext)
{
  public async Task<CancelOperationResponse> Handle(CompanyAccess companyAccess, BonusId bonusId)
  {
    var bonus = await bonusContext.GetBonus(CompanyId.From(companyAccess), bonusId);
    if (bonus is null)
    {
      throw new Exception("bonus not found");
    }

    await bonusContext.CancelBonus(bonus);

    return new CancelOperationResponse.Ok();
  }
}
