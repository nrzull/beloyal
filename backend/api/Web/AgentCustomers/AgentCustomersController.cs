using Beloyal.API.Companies;
using Beloyal.API.Shops;
using Beloyal.API.Web.AgentCustomers.AddBonus;
using Beloyal.API.Web.AgentCustomers.GetCustomerByPublicToken;
using Beloyal.API.Web.AgentCustomers.SubtractBonus;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.AgentCustomers;

[ApiController]
[Route("/api/agent/customers")]
[WithAuth]
[WithCompanyAccess]
[WithShopAccess]
public class AgentCustomersController : ControllerBase
{
  [HttpGet("public-tokens/{publicToken}")]
  [ProducesResponseType(typeof(GetCustomerByPublicTokenResponse.Ok), StatusCodes.Status200OK)]
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

  [HttpPost("public-tokens/{publicToken}/add-bonus")]
  [ProducesResponseType(typeof(AddBonusResponse.Ok), StatusCodes.Status200OK)]
  public async Task<Results<Ok<AddBonusResponse.Ok>, BadRequest>> AddBonus(
    [FromServices] AddBonusEndpoint endpoint,
    [FromServices] ShopAccess? shopAccess,
    [FromServices] Shop? shop,
    [FromRoute(Name = "publicToken")] string publicToken,
    [FromBody] AddBonusRequest req
  ) =>
    await endpoint.Handle(shopAccess.Ensure(), shop.Ensure(), publicToken, req) switch
    {
      AddBonusResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost("public-tokens/{publicToken}/subtract-bonus")]
  [ProducesResponseType(typeof(SubtractBonusResponse.Ok), StatusCodes.Status200OK)]
  public async Task<Results<Ok<SubtractBonusResponse.Ok>, BadRequest>> SubtractBonus(
    [FromServices] SubtractBonusEndpoint endpoint,
    [FromServices] ShopAccess? shopAccess,
    [FromServices] Shop? shop,
    [FromRoute(Name = "publicToken")] string publicToken,
    [FromBody] SubtractBonusRequest req
  ) =>
    await endpoint.Handle(shopAccess.Ensure(), shop.Ensure(), publicToken, req) switch
    {
      SubtractBonusResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
