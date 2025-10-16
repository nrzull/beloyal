using Beloyal.API.Companies;
using Beloyal.API.Promotions;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Promotions.CreatePromotion;
using Beloyal.API.Web.Promotions.GetPromotion;
using Beloyal.API.Web.Promotions.GetPromotions;
using Beloyal.API.Web.Promotions.UpdatePromotion;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Promotions;

[ApiController]
[Route("/api/company/promotions")]
[WithAuth]
[WithCompanyAccess]
public class PromotionsController : ControllerBase
{
  [HttpGet]
  public async Task<Results<Ok<GetPromotionsResponse.Ok>, BadRequest>> GetPromotions(
    [FromServices] GetPromotionsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetPromotionsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}")]
  public async Task<Results<Ok<GetPromotionResponse.Ok>, BadRequest, NotFound>> GetPromotion(
    [FromServices] GetPromotionEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid promotionId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), PromotionId.From(promotionId)) switch
    {
      GetPromotionResponse.Ok ok => TypedResults.Ok(ok),
      GetPromotionResponse.NotFound => TypedResults.NotFound(),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost]
  public async Task<Results<Ok<CreatePromotionResponse.Ok>, BadRequest>> CreatePromotion(
    [FromServices] CreatePromotionEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreatePromotionRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreatePromotionResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("{id}")]
  public async Task<Results<Ok<UpdatePromotionResponse.Ok>, BadRequest>> UpdatePromotion(
    [FromServices] UpdatePromotionEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid promotionId,
    [FromBody] CreatePromotionRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), PromotionId.From(promotionId), req) switch
    {
      UpdatePromotionResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
