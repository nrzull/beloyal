using Beloyal.API.Companies;
using Beloyal.API.Levels;

namespace Beloyal.API.Web.Levels.CreateLevel;

public class CreateLevelEndpoint(LevelContext levelContext)
{
  public async Task<CreateLevelResponse> Handle(CompanyAccess companyAccess, CreateLevelRequest req)
  {
    var level = await levelContext.CreateLevel(
      companyAccess.Company,
      new Types.Percent(req.CashbackPercent),
      req.PointsCapacity,
      req.Description
    );

    return new CreateLevelResponse.Ok(LevelDTO.From(level));
  }
}
