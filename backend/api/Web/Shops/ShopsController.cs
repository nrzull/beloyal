using Beloyal.API.Companies;
using Beloyal.API.Shops;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Shops.CreateShop;
using Beloyal.API.Web.Shops.CreateShopAccess;
using Beloyal.API.Web.Shops.GetShop;
using Beloyal.API.Web.Shops.GetShopAccesses;
using Beloyal.API.Web.Shops.GetShops;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Shops;

[ApiController]
[Route("/api/company/shops")]
[WithAuth]
[WithCompanyAccess]
public class ShopsController : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetShops(
    [FromServices] GetShopsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetShopsResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpPost]
  public async Task<IActionResult> CreateShop(
    [FromServices] CreateShopEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateShopRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateShopResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpGet("{id}")]
  public async Task<IActionResult> GetShop(
    [FromServices] GetShopEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid shopId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ShopId.From(shopId)) switch
    {
      GetShopResponse.NotFound => NotFound(),
      GetShopResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpGet("{id}/shop-accesses")]
  public async Task<IActionResult> GetShopAccesses(
    [FromServices] GetShopAccessesEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid shopId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ShopId.From(shopId)) switch
    {
      GetShopAccessesResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpPost("{id}/shop-accesses")]
  public async Task<IActionResult> CreateShopAccess(
    [FromServices] CreateShopAccessEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateShopAccessRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateShopAccessResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
