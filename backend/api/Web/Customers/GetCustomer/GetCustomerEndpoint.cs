using Beloyal.API.Companies;
using Beloyal.API.Customers;

namespace Beloyal.API.Web.Customers.GetCustomer;

public class GetCustomerEndpoint(CustomerContext customerContext)
{
  public async Task<GetCustomerResponse> Handle(CompanyAccess companyAccess, CustomerId customerId)
  {
    var customer = await customerContext.GetCustomer(CompanyId.From(companyAccess), customerId);

    if (customer is null)
    {
      return new GetCustomerResponse.NotFound();
    }

    return new GetCustomerResponse.Ok(CustomerDTO.From(customer));
  }
}
