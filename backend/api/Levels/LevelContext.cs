using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Types;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Levels;

public class LevelContext(ApplicationDbContext dbContext, ILogger<LevelContext> logger)
{
  public async Task DowngradeLevel(Customer customer)
  {
    var currentLevel = await GetLevel(customer);
    if (currentLevel is null)
    {
      logger.LogInformation("no level for Customer {CustomerId}", customer.Id);
      return;
    }

    decimal newPoints = customer.LevelPointsAmount;
    Level prevLevel = currentLevel;

    var levels = await GetLevels(CompanyId.From(customer));

    var idx = levels.FindIndex((v) => v.Id == currentLevel.Id);
    if (idx != -1)
    {
      prevLevel = idx > 0 ? levels[idx - 1] : levels[0];
    }

    if (prevLevel.Id == currentLevel.Id)
    {
      newPoints = 0;
    }
    else
    {
      newPoints = prevLevel.PointsCapacity - 1;
    }

    if (customer.LevelPointsAmount != newPoints)
    {
      dbContext.Update(customer);
      customer.LevelPointsAmount = newPoints;
      await dbContext.SaveChangesAsync();

      logger.LogInformation(
        "downgraded level for Customer {CustomerId}. New points: {newPoints}",
        customer.Id,
        newPoints
      );
    }
  }

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

  public async Task<Level?> GetLevel(Customer customer)
  {
    var levels = await GetLevels(CompanyId.From(customer));
    var last = levels.LastOrDefault();

    Level? level = default;
    foreach (var l in levels)
    {
      if (customer.LevelPointsAmount <= l.PointsCapacity)
      {
        level = l;
        break;
      }
    }

    if (customer.LevelPointsAmount >= last?.PointsCapacity)
    {
      level = last;
    }

    return level;
  }

  public async Task<Level> CreateLevel(
    Company company,
    Percent cashbackPercent,
    decimal pointsCapacity,
    string? description
  )
  {
    if (pointsCapacity <= 0)
    {
      logger.LogError("pointsCapacity should be more than 0. Company: {CompanyId}", company.Id);
      throw new Exception();
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

    logger.LogInformation(
      "created new level for company {CompanyId}  with pointsCapacity: {pointsCapacity}",
      company.Id,
      pointsCapacity
    );

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
