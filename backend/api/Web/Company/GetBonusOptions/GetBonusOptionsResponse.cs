using Beloyal.API.Companies;

namespace Beloyal.API.Web.Company.GetBonusOptions;

public abstract record GetBonusOptionsResponse
{
  public record Ok(CompanyBonusOptionsDTO Data) : GetBonusOptionsResponse;
}
