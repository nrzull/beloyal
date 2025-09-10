using Beloyal.API.Companies;
using Beloyal.API.Customers;

namespace Beloyal.API.Web.AgentCustomers.GetCustomerByPublicToken;

public class GetCustomerByPublicTokenEndpoint(CustomerContext customerContext)
{
  public async Task<GetCustomerByPublicTokenResponse> Handle(
    CompanyAccess companyAccess,
    string publicToken
  )
  {
    var customer = await customerContext.GetCustomerByPublicToken(
      CompanyId.From(companyAccess),
      publicToken
    );
    if (customer is null)
    {
      throw new Exception("invalid publicToken");
    }

    return new GetCustomerByPublicTokenResponse.Ok(
      new(
        BonusAmount: customer.BonusAmount,
        PhoneNumber: customer.User.Phone!,
        CustomerId: customer.Id
      )
    );
  }
}
