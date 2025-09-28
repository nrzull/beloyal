using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccesses.BlockAccess;

public class BlockAccessEndpoint(CompanyContext companyContext)
{
  public async Task<BlockAccessResponse> Handle(
    CompanyAccess companyAccess,
    CompanyAccessId targetId
  )
  {
    var target = await companyContext.GetAccess(targetId);
    if (target is null || target.CompanyId != companyAccess.CompanyId || target.BlockedAt is { })
    {
      throw new Exception("invalid target");
    }

    await companyContext.BlockAccess(target);

    return new BlockAccessResponse.Ok();
  }
}
