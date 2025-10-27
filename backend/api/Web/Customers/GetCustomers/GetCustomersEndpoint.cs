using Beloyal.API.Companies;
using Beloyal.API.Customers;

namespace Beloyal.API.Web.Customers.GetCustomers;

public class GetCustomersEndpoint(CustomerContext customerContext)
{
  public async Task<GetCustomersResponse> Handle(CompanyAccess companyAccess)
  {
    var customers = await customerContext.GetCustomers(CompanyId.From(companyAccess));

    return new GetCustomersResponse.Ok(customers.Select(CustomerDTO.From).ToList());
  }
}
