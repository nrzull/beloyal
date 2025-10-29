using Beloyal.API.Companies;
using Beloyal.API.Levels;

namespace Beloyal.API.Web.Levels.GetLevels;

public class GetLevelsEndpoint(LevelContext levelContext)
{
  public async Task<GetLevelsResponse> Handle(CompanyAccess companyAccess)
  {
    var levels = await levelContext.GetLevels(CompanyId.From(companyAccess));

    return new GetLevelsResponse.Ok(levels.Select(LevelDTO.From).ToList());
  }
}
