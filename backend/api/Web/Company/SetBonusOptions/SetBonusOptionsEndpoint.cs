using Beloyal.API.Companies;
using Beloyal.API.Types;

namespace Beloyal.API.Web.Company.SetBonusOptions;

public class SetBonusOptionsEndpoint(CompanyContext companyContext)
{
  public async Task<SetBonusOptionsResponse> Handle(
    CompanyAccess companyAccess,
    CompanyBonusOptionsDTO req
  )
  {
    var company = await companyContext.GetCompany(CompanyId.From(companyAccess));

    var bonusFrequency =
      req.BonusFrequencyCount is ushort count && req.BonusFrequencyInDays is ushort inDays
        ? new CompanyBonusFrequency() { Count = count, Frequency = TimeSpan.FromDays(inDays) }
        : null;

    await companyContext.UpdateCompanyBonusOptions(
      company,
      new(req.CashbackPercent),
      new(req.CashbackThreshold),
      new(req.BonusMultiplier),
      TimeSpan.FromDays(req.BonusLifetimeInDays),
      bonusFrequency
    );

    return new SetBonusOptionsResponse.Ok(CompanyBonusOptionsDTO.From(company));
  }
}
