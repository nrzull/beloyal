using Beloyal.API.Companies;

namespace Beloyal.API.Web.WhiteCompanies.GetWhiteCompany;

public class GetWhiteCompanyEndpoint(CompanyContext companyContext)
{
  public async Task<GetWhiteCompanyResponse> Handle(WhiteCompanyId id)
  {
    var whiteCompany = await companyContext.GetWhiteCompany(id);
    if (whiteCompany is null)
    {
      return new GetWhiteCompanyResponse.NotFound();
    }

    return new GetWhiteCompanyResponse.Ok(WhiteCompanyDTO.From(whiteCompany));
  }
}
