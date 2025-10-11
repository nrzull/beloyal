using System.Text.Json;
using Beloyal.API.Promotions.Variants;

namespace Beloyal.API.Test.Promotions;

public class BaseVariantJsonSerializerContextTest
{
  [Fact]
  public void CountVariant()
  {
    var countVariant = new CountVariant { Count = 2, CashbackPercent = 5 };

    var ser = JsonSerializer.Serialize(
      countVariant,
      BaseVariantJsonSerializerContext.Default.BaseVariant
    );
    var deser = JsonSerializer.Deserialize(
      ser,
      BaseVariantJsonSerializerContext.Default.BaseVariant
    );

    Assert.True(deser is CountVariant { Count: 2, CashbackPercent: 5 });
  }
}
