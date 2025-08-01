using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccesses.RemoveAccess;

public class RemoveAccessEndpoint(CompanyContext companyContext)
{
  public async Task<RemoveAccessResponse> Handle(
    CompanyAccess companyAccess,
    RemoveAccessRequest req
  )
  {
    var access = await companyContext.GetAccess(CompanyAccessId.From(req.CompanyAccessId));
    if (access is null || companyAccess.CompanyId != access?.CompanyId)
    {
      throw new Exception("invalid access");
    }

    await companyContext.RemoveCompanyAccess(access);

    return new RemoveAccessResponse.Ok();
  }
}
