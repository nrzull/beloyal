using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccesses.GetAccess;

public class GetAccessEndpoint(CompanyContext companyContext)
{
  public async Task<GetAccessResponse> Handle(CompanyAccess access, CompanyAccessId id)
  {
    var result = await companyContext.GetAccess(CompanyId.From(access), id);
    if (result is null)
    {
      return new GetAccessResponse.NotFound();
    }

    return new GetAccessResponse.Ok(CompanyAccessDTO.From(result));
  }
}
