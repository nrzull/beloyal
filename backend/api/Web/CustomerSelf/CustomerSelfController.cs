using Beloyal.API.Customers;
using Beloyal.API.Web.CustomerSelf.GetCustomerSelf;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.CustomerSelf;

[ApiController]
[Route("/api/customer/self")]
[WithCustomerAuth]
[WithCustomer]
public class CustomerSelfController : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetCustomerSelf(
    [FromServices] GetCustomerSelfEndpoint endpoint,
    [FromServices] Customer? customer
  ) =>
    await endpoint.Handle(customer.Ensure()) switch
    {
      GetCustomerSelfResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
