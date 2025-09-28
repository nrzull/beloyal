using Beloyal.API.Companies;

namespace Beloyal.API.Web.Company.SetBonusOptions;

public abstract record SetBonusOptionsResponse
{
  public record Ok(CompanyBonusOptionsDTO Data) : SetBonusOptionsResponse;
}
