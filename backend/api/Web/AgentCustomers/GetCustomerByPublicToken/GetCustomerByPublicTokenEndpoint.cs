using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Customers;

namespace Beloyal.API.Web.AgentCustomers.GetCustomerByPublicToken;

public class GetCustomerByPublicTokenEndpoint(
  AuthContext authContext,
  CustomerContext customerContext
)
{
  public async Task<GetCustomerByPublicTokenResponse> Handle(
    CompanyAccess companyAccess,
    string publicToken
  )
  {
    var decoded = authContext.DecodeCustomerPublicAccessToken(publicToken);
    if (decoded is null)
    {
      throw new Exception("invalid publicToken");
    }

    var customer = await customerContext.GetCustomer(CustomerId.From(decoded.CustomerId));
    if (customer is null || customer.CompanyId != companyAccess.CompanyId)
    {
      throw new Exception("invalid customer");
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
