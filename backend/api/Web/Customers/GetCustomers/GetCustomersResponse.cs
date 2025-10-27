using Beloyal.API.Customers;

namespace Beloyal.API.Web.Customers.GetCustomers;

public abstract record GetCustomersResponse
{
  public record Ok(List<CustomerDTO> Data) : GetCustomersResponse;
}
