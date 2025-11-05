namespace Beloyal.API.Web.Analytics.GetGeneralAnalytics;

public record GetGeneralAnalyticsRequest(GetGeneralAnalyticsRequestFilters Filters);

public record GetGeneralAnalyticsRequestFilters(
  DateTimeOffset From,
  DateTimeOffset To,
  Guid? ShopId
);
