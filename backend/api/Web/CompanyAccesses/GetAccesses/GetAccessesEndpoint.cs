using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccesses.GetAccesses;

public class GetAccessesEndpoint(CompanyContext companyContext)
{
  public async Task<GetAccessesResponse> Handle(CompanyAccess access)
  {
    var accesses = await companyContext.GetCompanyAccesses(CompanyId.From(access.CompanyId));

    return new GetAccessesResponse.Ok(accesses.Select(CompanyAccessDTO.From).ToList());
  }
}
