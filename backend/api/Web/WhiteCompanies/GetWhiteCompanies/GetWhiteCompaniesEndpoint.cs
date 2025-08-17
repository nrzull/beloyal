using Beloyal.API.Companies;

namespace Beloyal.API.Web.WhiteCompanies.GetWhiteCompanies;

public class GetWhiteCompaniesEndpoint(CompanyContext companyContext)
{
  public async Task<GetWhiteCompaniesResponse> Handle()
  {
    var result = await companyContext.GetWhiteCompanies();

    return new GetWhiteCompaniesResponse.Ok(result.Select(WhiteCompanyDTO.From).ToList());
  }
}
