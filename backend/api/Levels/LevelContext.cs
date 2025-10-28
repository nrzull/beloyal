using Beloyal.API.Companies;
using Beloyal.API.Types;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Levels;

public class LevelContext(ApplicationDbContext dbContext)
{
  public async Task<List<Level>> GetLevels(CompanyId companyId)
  {
    var levels = await dbContext.Levels.Where(v => v.CompanyId == companyId.Value).ToListAsync();

    levels.Sort((v1, v2) => v1.PointsCapacity < v2.PointsCapacity ? -1 : 1);

    return levels;
  }

  public async Task<Level?> GetLevel(CompanyId companyId, LevelId levelId) =>
    await dbContext.Levels.FirstOrDefaultAsync(v =>
      v.CompanyId == companyId.Value && v.Id == levelId.Value
    );

  public async Task<Level> CreateLevel(
    Company company,
    Percent cashbackPercent,
    decimal pointsCapacity,
    string? description
  )
  {
    if (pointsCapacity <= 0)
    {
      throw new Exception("pointsCapacity should be more than 0");
    }

    var level = new Level
    {
      CashbackPercent = cashbackPercent.Value,
      Company = company,
      PointsCapacity = pointsCapacity,
      Description = description,
    };

    dbContext.Add(level);
    await dbContext.SaveChangesAsync();

    return level;
  }

  public async Task UpdateLevel(
    Level level,
    Percent cashbackPercent,
    decimal pointsCapacity,
    string? description
  )
  {
    if (pointsCapacity <= 0)
    {
      throw new Exception("pointsCapacity should be more than 0");
    }

    dbContext.Update(level);
    level.CashbackPercent = cashbackPercent.Value;
    level.PointsCapacity = pointsCapacity;
    level.Description = description;

    await dbContext.SaveChangesAsync();
  }

  public async Task DeleteLevel(LevelId levelId)
  {
    await dbContext.Levels.Where(v => v.Id == levelId.Value).ExecuteDeleteAsync();
  }
}
