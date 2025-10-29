using Beloyal.API.Companies;
using Beloyal.API.Promotions;
using Beloyal.API.Shops;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Shops.AddPromotion;
using Beloyal.API.Web.Shops.CreateShop;
using Beloyal.API.Web.Shops.CreateShopAccess;
using Beloyal.API.Web.Shops.GetPromotions;
using Beloyal.API.Web.Shops.GetShop;
using Beloyal.API.Web.Shops.GetShopAccesses;
using Beloyal.API.Web.Shops.GetShops;
using Beloyal.API.Web.Shops.RemovePromotion;
using Beloyal.API.Web.Shops.RemoveShopAccess;
using Beloyal.API.Web.Shops.UpdateShop;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Shops;

[ApiController]
[Route("/api/company/shops")]
[WithAuth]
[WithCompanyAccess]
public class ShopsController : ControllerBase
{
  [HttpGet]
  [WithCompanyPermissions(nameof(CompanyPermissions.shops_read))]
  [ProducesResponseType(typeof(GetShopsResponse.Ok), StatusCodes.Status200OK)]
  public async Task<Results<Ok<GetShopsResponse.Ok>, BadRequest>> GetShops(
    [FromServices] GetShopsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetShopsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost]
  [WithCompanyPermissions(nameof(CompanyPermissions.shops_write))]
  [ProducesResponseType(typeof(CreateShopResponse.Ok), StatusCodes.Status200OK)]
  public async Task<Results<Ok<CreateShopResponse.Ok>, BadRequest>> CreateShop(
    [FromServices] CreateShopEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateShopRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateShopResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}")]
  [WithCompanyPermissions(nameof(CompanyPermissions.shops_read))]
  [ProducesResponseType(typeof(GetShopResponse.Ok), StatusCodes.Status200OK)]
  public async Task<Results<Ok<GetShopResponse.Ok>, NotFound, BadRequest>> GetShop(
    [FromServices] GetShopEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid shopId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ShopId.From(shopId)) switch
    {
      GetShopResponse.NotFound => TypedResults.NotFound(),
      GetShopResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("{id}")]
  [WithCompanyPermissions(nameof(CompanyPermissions.shops_write))]
  public async Task<Results<Ok<UpdateShopResponse.Ok>, BadRequest>> UpdateShop(
    [FromServices] UpdateShopEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid shopId,
    [FromBody] UpdateShopRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ShopId.From(shopId), req) switch
    {
      UpdateShopResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}/shop-accesses")]
  [WithCompanyPermissions(nameof(CompanyPermissions.shop_accesses_read))]
  [ProducesResponseType(typeof(GetShopAccessesResponse.Ok), StatusCodes.Status200OK)]
  public async Task<Results<Ok<GetShopAccessesResponse.Ok>, BadRequest>> GetShopAccesses(
    [FromServices] GetShopAccessesEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid shopId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ShopId.From(shopId)) switch
    {
      GetShopAccessesResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost("{id}/shop-accesses")]
  [WithCompanyPermissions(nameof(CompanyPermissions.shop_accesses_write))]
  [ProducesResponseType(typeof(CreateShopAccessResponse.Ok), StatusCodes.Status200OK)]
  public async Task<Results<Ok<CreateShopAccessResponse.Ok>, BadRequest>> CreateShopAccess(
    [FromServices] CreateShopAccessEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateShopAccessRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateShopAccessResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpDelete("{id}/shop-accesses/{shopAccessId}")]
  [WithCompanyPermissions(nameof(CompanyPermissions.shop_accesses_write))]
  public async Task<Results<Ok<RemoveShopAccessResponse.Ok>, BadRequest>> RemoveShopAccess(
    [FromServices] RemoveShopAccessEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "shopAccessId")] Guid shopAccessId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ShopAccessId.From(shopAccessId)) switch
    {
      RemoveShopAccessResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}/shop-promotions")]
  [WithCompanyPermissions(nameof(CompanyPermissions.shop_promotions_read))]
  public async Task<Results<Ok<GetPromotionsResponse.Ok>, BadRequest>> GetShopPromotions(
    [FromServices] GetPromotionsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid shopId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ShopId.From(shopId)) switch
    {
      GetPromotionsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost("{id}/shop-promotions")]
  [WithCompanyPermissions(nameof(CompanyPermissions.shop_promotions_write))]
  public async Task<Results<Ok<AddPromotionResponse.Ok>, BadRequest>> AddPromotion(
    [FromServices] AddPromotionEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid shopId,
    [FromBody] AddPromotionRequest req
  ) =>
    await endpoint.Handle(
      companyAccess.Ensure(),
      ShopId.From(shopId),
      PromotionId.From(req.PromotionId)
    ) switch
    {
      AddPromotionResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpDelete("{id}/shop-promotions/{shopPromotionId}")]
  [WithCompanyPermissions(nameof(CompanyPermissions.shop_promotions_write))]
  public async Task<Results<Ok<RemovePromotionResponse.Ok>, BadRequest>> RemovePromotion(
    [FromServices] RemovePromotionEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "shopPromotionId")] Guid shopPromotionId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ShopPromotionId.From(shopPromotionId)) switch
    {
      RemovePromotionResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
