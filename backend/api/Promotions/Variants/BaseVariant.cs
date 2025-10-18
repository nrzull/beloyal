using System.Text.Json.Serialization;

namespace Beloyal.API.Promotions.Variants;

[JsonPolymorphic]
[JsonDerivedType(typeof(DatePeriodVariant), nameof(DatePeriodVariant))]
[JsonDerivedType(typeof(TimePeriodVariant), nameof(TimePeriodVariant))]
[JsonDerivedType(typeof(DateTimePeriodVariant), nameof(DateTimePeriodVariant))]
[JsonDerivedType(typeof(ExactCountVariant), nameof(ExactCountVariant))]
public class BaseVariant
{
  public virtual required decimal CashbackPercent { get; set; }
};

[JsonSourceGenerationOptions(
  WriteIndented = false,
  PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
  PropertyNameCaseInsensitive = true,
  RespectNullableAnnotations = true,
  RespectRequiredConstructorParameters = true,
  AllowOutOfOrderMetadataProperties = true,
  UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
)]
[JsonSerializable(typeof(List<DatePeriodVariant>))]
[JsonSerializable(typeof(List<TimePeriodVariant>))]
[JsonSerializable(typeof(List<DateTimePeriodVariant>))]
[JsonSerializable(typeof(List<ExactCountVariant>))]
[JsonSerializable(typeof(List<BaseVariant>))]
public partial class BaseVariantJsonSerializerContext : JsonSerializerContext;
