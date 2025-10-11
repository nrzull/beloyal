using System.Text.Json;
using Beloyal.API.Promotions.Variants;

namespace Beloyal.API.Promotions;

public record PromotionDTO(Guid Id, string Title, DateTimeOffset CreatedAt, JsonElement Variant)
{
  public static PromotionDTO From(Promotion v) =>
    new(
      Id: v.Id,
      CreatedAt: v.CreatedAt,
      Title: v.Title,
      Variant: JsonSerializer.SerializeToElement(
        v.Variant,
        BaseVariantJsonSerializerContext.Default.BaseVariant
      )
    );
};
