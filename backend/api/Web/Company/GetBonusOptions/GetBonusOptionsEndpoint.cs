using Beloyal.API.Companies;

namespace Beloyal.API.Web.Company.GetBonusOptions;

public class GetBonusOptionsEndpoint(CompanyContext companyContext)
{
  public async Task<GetBonusOptionsResponse> Handle(CompanyAccess companyAccess)
  {
    var company = await companyContext.GetCompany(CompanyId.From(companyAccess));

    return new GetBonusOptionsResponse.Ok(CompanyBonusOptionsDTO.From(company));
  }
}
