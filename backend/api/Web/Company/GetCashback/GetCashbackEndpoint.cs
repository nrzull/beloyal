using Beloyal.API.Companies;

namespace Beloyal.API.Web.Company.GetCashback;

public class GetCashbackEndpoint(CompanyContext companyContext)
{
  public async Task<GetCashbackResponse> Handle(CompanyAccess companyAccess)
  {
    var company = await companyContext.GetCompany(CompanyId.From(companyAccess));

    return new GetCashbackResponse.Ok(CompanyCashbackDTO.From(company));
  }
}
