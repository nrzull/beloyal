using Beloyal.API.Companies;
using Beloyal.API.Customers;

namespace Beloyal.API.Web.CustomerSelf.GetCustomerSelf;

public class GetCustomerSelfEndpoint(CompanyContext companyContext)
{
  public async Task<GetCustomerSelfResponse> Handle(Customer customer)
  {
    var company = await companyContext.GetCompany(CompanyId.From(customer));
    if (company is null)
    {
      throw new Exception("invalid company");
    }

    return new GetCustomerSelfResponse.Ok(
      new(
        CompanyName: customer.Company.Tin,
        BonusAmount: customer.BonusAmount,
        BonusMultiplier: company.BonusMultiplier
      )
    );
  }
}
