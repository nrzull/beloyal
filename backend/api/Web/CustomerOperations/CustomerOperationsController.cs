using Beloyal.API.Customers;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Operations.GetOperations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using GetCustomerOperationsEndpoint = Beloyal.API.Web.CustomerOperations.GetOperations.GetOperationsEndpoint;

namespace Beloyal.API.Web.CustomerOperations;

[ApiController]
[Route("/api/customer/operations")]
[WithCustomerAuth]
[WithCustomer]
public class CustomerOperationsController : ControllerBase
{
  [HttpGet]
  public async Task<Results<Ok<GetOperationsResponse.Ok>, BadRequest>> GetOperations(
    [FromServices] GetCustomerOperationsEndpoint endpoint,
    [FromServices] Customer? customer
  ) =>
    await endpoint.Handle(customer.Ensure()) switch
    {
      GetOperationsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
