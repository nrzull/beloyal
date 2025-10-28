using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Levels;

namespace Beloyal.API.Web.CustomerSelf.GetCustomerSelf;

public class GetCustomerSelfEndpoint(CompanyContext companyContext, LevelContext levelContext)
{
  public async Task<GetCustomerSelfResponse> Handle(Customer customer)
  {
    var company = await companyContext.GetCompany(CompanyId.From(customer));
    if (company is null)
    {
      throw new Exception("invalid company");
    }

    var levels = await levelContext.GetLevels(CompanyId.From(company));

    return new GetCustomerSelfResponse.Ok(
      new(
        CompanyName: customer.Company.Tin,
        BonusAmount: customer.BonusAmount,
        BonusMultiplier: company.BonusMultiplier,
        LevelPointsAmount: customer.LevelPointsAmount,
        CompanyLevels: levels.Select(LevelDTO.From).ToList()
      )
    );
  }
}
