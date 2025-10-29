using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Web.Customers.CreateBooster;
using Beloyal.API.Web.Customers.GetCustomer;
using Beloyal.API.Web.Customers.GetCustomerBoosters;
using Beloyal.API.Web.Customers.GetCustomerOperations;
using Beloyal.API.Web.Customers.GetCustomers;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Operations.GetOperations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Customers;

[ApiController]
[Route("/api/company/customers")]
[WithAuth]
[WithCompanyAccess]
public class CustomersController : ControllerBase
{
  [HttpGet]
  [WithCompanyPermissions(nameof(CompanyPermissions.customers_read))]
  public async Task<Results<Ok<GetCustomersResponse.Ok>, BadRequest>> GetCustomers(
    [FromServices] GetCustomersEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetCustomersResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}")]
  [WithCompanyPermissions(nameof(CompanyPermissions.customers_read))]
  public async Task<Results<Ok<GetCustomerResponse.Ok>, NotFound, BadRequest>> GetCustomer(
    [FromServices] GetCustomerEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid customerId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), CustomerId.From(customerId)) switch
    {
      GetCustomerResponse.Ok ok => TypedResults.Ok(ok),
      GetCustomerResponse.NotFound => TypedResults.NotFound(),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}/operations")]
  [WithCompanyPermissions(nameof(CompanyPermissions.operations_read))]
  public async Task<Results<Ok<GetOperationsResponse.Ok>, BadRequest>> GetCustomerOperations(
    [FromServices] GetCustomerOperationsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid customerId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), CustomerId.From(customerId)) switch
    {
      GetOperationsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}/boosters")]
  [WithCompanyPermissions(nameof(CompanyPermissions.boosters_read))]
  public async Task<Results<Ok<GetCustomerBoostersResponse.Ok>, BadRequest>> GetCustomerBoosters(
    [FromServices] GetCustomerBoostersEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid customerId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), CustomerId.From(customerId)) switch
    {
      GetCustomerBoostersResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost("{id}/boosters")]
  [WithCompanyPermissions(nameof(CompanyPermissions.boosters_write))]
  public async Task<Results<Ok<CreateBoosterResponse.Ok>, BadRequest>> CreateBooster(
    [FromServices] CreateBoosterEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid customerId,
    [FromBody] CreateBoosterRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), CustomerId.From(customerId), req) switch
    {
      CreateBoosterResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
