namespace Beloyal.API.Countries;

public class CountryContext
{
  public List<CountryLang> GetCountryLangs(CountryCode cc) =>
    CountryLangs.Values.FirstOrDefault(v => v.Key == cc).Value ?? [CountryLang.ru_RU];

  public List<string> GetCountryTimeZones(CountryCode cc) =>
    CountryTimeZones.Values.FirstOrDefault(v => v.Key == cc).Value ?? [];
}
