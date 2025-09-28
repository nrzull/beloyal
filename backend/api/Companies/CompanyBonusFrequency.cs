namespace Beloyal.API.Companies;

public class CompanyBonusFrequency
{
  public required ushort Count { get; set; }
  public required TimeSpan Frequency { get; set; }
}
