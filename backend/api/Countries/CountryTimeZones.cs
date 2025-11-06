namespace Beloyal.API.Countries;

public static class CountryTimeZones
{
  public static Dictionary<CountryCode, List<string>> Values = new()
  {
    {
      CountryCode.RU,
      [
        "Europe/Kaliningrad",
        "Europe/Moscow",
        "Europe/Kirov",
        "Europe/Volgograd",
        "Europe/Astrakhan",
        "Europe/Saratov",
        "Europe/Ulyanovsk",
        "Europe/Samara",
        "Asia/Yekaterinburg",
        "Asia/Omsk",
        "Asia/Novosibirsk",
        "Asia/Barnaul",
        "Asia/Tomsk",
        "Asia/Novokuznetsk",
        "Asia/Krasnoyarsk",
        "Asia/Irkutsk",
        "Asia/Chita",
        "Asia/Yakutsk",
        "Asia/Khandyga",
        "Asia/Vladivostok",
        "Asia/Ust-Nera",
        "Asia/Magadan",
        "Asia/Sakhalin",
        "Asia/Srednekolymsk",
        "Asia/Kamchatka",
        "Asia/Anadyr",
      ]
    },
    {
      CountryCode.KZ,
      [
        "Asia/Almaty",
        "Asia/Qyzylorda",
        "Asia/Qostanay",
        "Asia/Aqtobe",
        "Asia/Aqtau",
        "Asia/Atyrau",
        "Asia/Oral",
      ]
    },
    { CountryCode.KG, ["Asia/Bishkek"] },
    { CountryCode.UZ, ["Asia/Samarkand", "Asia/Tashkent"] },
    { CountryCode.BY, ["Europe/Minsk"] },
  };
}
