using Beloyal.API.Countries;
using Beloyal.API.Web.Countries.GetCountries;
using Beloyal.API.Web.Countries.GetCountryLangs;
using Beloyal.API.Web.Countries.GetCountryTimeZones;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Countries;

[ApiController]
[Route("/api/countries")]
[WithAuth]
public class CountriesController : ControllerBase
{
  [HttpGet]
  public Ok<GetCountriesResponse.Ok> GetCountries([FromServices] GetCountriesEndpoint endpoint) =>
    TypedResults.Ok(endpoint.Handle());

  [HttpGet("{countryCode}/langs")]
  public Ok<GetCountryLangsResponse.Ok> GetCountryLangs(
    [FromServices] GetCountryLangsEndpoint endpoint,
    [FromRoute(Name = "countryCode")] CountryCode cc
  ) => TypedResults.Ok(endpoint.Handle(cc));

  [HttpGet("{countryCode}/timeZones")]
  public Ok<GetCountryTimeZonesResponse.Ok> GetCountryTimeZones(
    [FromServices] GetCountryTimeZonesEndpoint endpoint,
    [FromRoute(Name = "countryCode")] CountryCode cc
  ) => TypedResults.Ok(endpoint.Handle(cc));
}
