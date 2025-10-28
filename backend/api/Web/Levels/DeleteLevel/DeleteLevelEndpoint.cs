using Beloyal.API.Companies;
using Beloyal.API.Levels;

namespace Beloyal.API.Web.Levels.DeleteLevel;

public class DeleteLevelEndpoint(LevelContext levelContext)
{
  public async Task<DeleteLevelResponse> Handle(CompanyAccess companyAccess, LevelId levelId)
  {
    var level = await levelContext.GetLevel(CompanyId.From(companyAccess), levelId);
    if (level is null)
    {
      throw new Exception("invalid level");
    }

    await levelContext.DeleteLevel(LevelId.From(level));

    return new DeleteLevelResponse.Ok();
  }
}
