using Beloyal.API.Companies;

namespace Beloyal.API.Web.Company.SetCashback;

public class SetCashbackEndpoint(CompanyContext companyContext)
{
  public async Task<SetCashbackResponse> Handle(CompanyAccess companyAccess, CompanyCashbackDTO req)
  {
    var company = await companyContext.GetCompany(CompanyId.From(companyAccess));

    await companyContext.UpdateCompanyCashback(
      company,
      new(req.CashbackPercent),
      new(req.CashbackThreshold),
      new(req.BonusMultiplier),
      TimeSpan.FromDays(req.BonusLifetimeInDays)
    );

    return new SetCashbackResponse.Ok(CompanyCashbackDTO.From(company));
  }
}
