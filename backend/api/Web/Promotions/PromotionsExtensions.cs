using Beloyal.API.Web.Promotions.CreatePromotion;
using Beloyal.API.Web.Promotions.GetPromotions;

namespace Beloyal.API.Web.Promotions;

public static class PromotionsExtensions
{
  public static void UsePromotionsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetPromotionsEndpoint>();
    builder.Services.AddScoped<CreatePromotionEndpoint>();
  }
}
