using Beloyal.API.Web.Promotions.CreatePromotion;
using Beloyal.API.Web.Promotions.GetPromotion;
using Beloyal.API.Web.Promotions.GetPromotions;
using Beloyal.API.Web.Promotions.UpdatePromotion;

namespace Beloyal.API.Web.Promotions;

public static class PromotionsExtensions
{
  public static void UsePromotionsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetPromotionsEndpoint>();
    builder.Services.AddScoped<CreatePromotionEndpoint>();
    builder.Services.AddScoped<GetPromotionEndpoint>();
    builder.Services.AddScoped<UpdatePromotionEndpoint>();
  }
}
