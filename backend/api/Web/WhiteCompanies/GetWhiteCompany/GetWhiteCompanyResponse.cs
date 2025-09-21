using Beloyal.API.Companies;

namespace Beloyal.API.Web.WhiteCompanies.GetWhiteCompany;

public abstract record GetWhiteCompanyResponse
{
  public record Ok(WhiteCompanyDTO Data) : GetWhiteCompanyResponse;

  public record NotFound : GetWhiteCompanyResponse;
}
