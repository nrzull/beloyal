namespace Beloyal.API.Countries;

public class CountryContext
{
  public List<string> GetCountryTimeZones(CountryCode cc) =>
    CountryTimeZones.Values.FirstOrDefault(v => v.Key == cc).Value ?? [];
}
