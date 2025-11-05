using System.Text.Json;
using Beloyal.API.Companies;
using Beloyal.API.Web.Analytics.GetGeneralAnalytics;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Analytics;

[ApiController]
[Route("/api/company/analytics")]
[WithAuth]
[WithCompanyAccess]
public class AnalyticsController : ControllerBase
{
  [HttpGet("general")]
  [WithCompanyPermissions(nameof(CompanyPermissions.analytics_read))]
  [WithCompanyPermissions(nameof(CompanyPermissions.shops_read))]
  public async Task<Results<Ok<GetGeneralAnalyticsResponse.Ok>, BadRequest>> GetGeneralAnalytics(
    [FromServices] GetGeneralAnalyticsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromQuery(Name = "filters")] string filters
  ) =>
    await endpoint.Handle(
      companyAccess.Ensure(),
      JsonSerializer.Deserialize<GetGeneralAnalyticsRequestFilters>(
        filters,
        JsonSerializerOptions.Web
      )!
    ) switch
    {
      GetGeneralAnalyticsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
