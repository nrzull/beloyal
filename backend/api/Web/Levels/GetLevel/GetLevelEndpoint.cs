using Beloyal.API.Companies;
using Beloyal.API.Levels;

namespace Beloyal.API.Web.Levels.GetLevel;

public class GetLevelEndpoint(LevelContext levelContext)
{
  public async Task<GetLevelResponse> Handle(CompanyAccess companyAccess, LevelId levelId)
  {
    var level = await levelContext.GetLevel(CompanyId.From(companyAccess), levelId);

    if (level is null)
    {
      return new GetLevelResponse.NotFound();
    }

    return new GetLevelResponse.Ok(LevelDTO.From(level));
  }
}
