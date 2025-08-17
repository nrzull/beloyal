using Beloyal.API.Companies;

namespace Beloyal.API.Web.WhiteCompanies.GetWhiteCompanies;

public abstract record GetWhiteCompaniesResponse
{
  public record Ok(List<WhiteCompanyDTO> Data) : GetWhiteCompaniesResponse;
}
