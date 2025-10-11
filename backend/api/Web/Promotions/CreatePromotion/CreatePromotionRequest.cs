using System.Text.Json;

namespace Beloyal.API.Web.Promotions.CreatePromotion;

public record CreatePromotionRequest(string Title, JsonElement Variant);
