using Beloyal.API.Companies;
using Beloyal.API.Levels;

namespace Beloyal.API.Web.Levels.UpdateLevel;

public class UpdateLevelEndpoint(LevelContext levelContext)
{
  public async Task<UpdateLevelResponse> Handle(
    CompanyAccess companyAccess,
    LevelId levelId,
    LevelDTO req
  )
  {
    var level = await levelContext.GetLevel(CompanyId.From(companyAccess), levelId);
    if (level is null)
    {
      throw new Exception("invalid level");
    }

    await levelContext.UpdateLevel(
      level,
      new Types.Percent(req.CashbackPercent),
      req.PointsCapacity,
      req.Description
    );

    return new UpdateLevelResponse.Ok(LevelDTO.From(level));
  }
}
