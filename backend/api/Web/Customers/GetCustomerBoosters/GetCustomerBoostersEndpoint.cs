using Beloyal.API.Boosters;
using Beloyal.API.Companies;
using Beloyal.API.Customers;

namespace Beloyal.API.Web.Customers.GetCustomerBoosters;

public class GetCustomerBoostersEndpoint(
  BoosterContext boosterContext,
  CustomerContext customerContext
)
{
  public async Task<GetCustomerBoostersResponse> Handle(
    CompanyAccess companyAccess,
    CustomerId customerId
  )
  {
    if (await customerContext.GetCustomer(CompanyId.From(companyAccess), customerId) is null)
    {
      throw new Exception("invalid customer");
    }

    var boosters = await boosterContext.GetBoosters(customerId);

    return new GetCustomerBoostersResponse.Ok(boosters.Select(BoosterDTO.From).ToList());
  }
}
