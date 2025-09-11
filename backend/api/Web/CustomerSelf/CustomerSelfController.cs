using Beloyal.API.Customers;
using Beloyal.API.Web.CustomerSelf.GetCustomerSelf;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.CustomerSelf;

[ApiController]
[Route("/api/customer/self")]
[WithCustomerAuth]
[WithCustomer]
public class CustomerSelfController : ControllerBase
{
  [HttpGet]
  public async Task<Results<BadRequest, Ok<GetCustomerSelfResponse.Ok>>> GetCustomerSelf(
    [FromServices] GetCustomerSelfEndpoint endpoint,
    [FromServices] Customer? customer
  ) =>
    await endpoint.Handle(customer.Ensure()) switch
    {
      GetCustomerSelfResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
