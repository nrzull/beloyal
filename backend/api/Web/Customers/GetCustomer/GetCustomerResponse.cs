using Beloyal.API.Customers;

namespace Beloyal.API.Web.Customers.GetCustomer;

public abstract record GetCustomerResponse
{
  public record Ok(CustomerDTO Data) : GetCustomerResponse;

  public record NotFound : GetCustomerResponse;
}
