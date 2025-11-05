using Beloyal.API.Web.Analytics.GetGeneralAnalytics;

namespace Beloyal.API.Web.Analytics;

public static class AnalyticsExtensions
{
  public static void UseAnalyticsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetGeneralAnalyticsEndpoint>();
  }
}
