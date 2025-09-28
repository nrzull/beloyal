using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccesses.UnblockAccess;

public class UnblockAccessEndpoint(CompanyContext companyContext)
{
  public async Task<UnblockAccessResponse> Handle(
    CompanyAccess companyAccess,
    CompanyAccessId targetId
  )
  {
    var target = await companyContext.GetAccess(targetId);
    if (target is null || target.CompanyId != companyAccess.CompanyId || target.BlockedAt is null)
    {
      throw new Exception("invalid target");
    }

    await companyContext.UnblockAccess(target);

    return new UnblockAccessResponse.Ok();
  }
}
