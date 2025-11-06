namespace Beloyal.API.Countries;

public enum CountryLang
{
  ru_RU,
  en_US,
}

public static class CountryLangs
{
  public static Dictionary<CountryCode, List<CountryLang>> Values = new()
  {
    { CountryCode.RU, [CountryLang.ru_RU, CountryLang.en_US] },
    { CountryCode.KZ, [CountryLang.ru_RU, CountryLang.en_US] },
    { CountryCode.KG, [CountryLang.ru_RU, CountryLang.en_US] },
    { CountryCode.UZ, [CountryLang.ru_RU, CountryLang.en_US] },
    { CountryCode.BY, [CountryLang.ru_RU, CountryLang.en_US] },
  };
}
