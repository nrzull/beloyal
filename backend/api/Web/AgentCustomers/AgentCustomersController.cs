using Beloyal.API.Companies;
using Beloyal.API.Web.AgentCustomers.GetCustomerByPublicToken;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.AgentCustomers;

[ApiController]
[Route("/api/agent/customers")]
[WithAuth]
[WithCompanyAccess]
public class AgentCustomersController : ControllerBase
{
  [HttpGet("public-tokens/{publicToken}")]
  public async Task<
    Results<Ok<GetCustomerByPublicTokenResponse.Ok>, BadRequest>
  > GetCustomerByPublicToken(
    [FromServices] GetCustomerByPublicTokenEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "publicToken")] string publicToken
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), publicToken) switch
    {
      GetCustomerByPublicTokenResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
