using Beloyal.API.Companies;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Web.Analytics.GetGeneralAnalytics;

public class GetGeneralAnalyticsEndpoint(ApplicationDbContext dbContext)
{
  public async Task<GetGeneralAnalyticsResponse> Handle(
    CompanyAccess companyAccess,
    GetGeneralAnalyticsRequestFilters filters
  )
  {
    var company = await dbContext.Companies.SingleAsync(v => v.Id == companyAccess.CompanyId);

    var utcFrom = filters.From.ToUniversalTime();
    var utcTo = filters.To.ToUniversalTime();

    var shop = filters.ShopId is { }
      ? await dbContext.Shops.FirstOrDefaultAsync(v =>
        v.Id == filters.ShopId
        && v.CompanyId == companyAccess.CompanyId
        && v.CreatedAt >= utcFrom
        && v.CreatedAt <= utcTo
      )
      : null;

    var tz = shop?.TimeZone ?? company.TimeZone;

    var bonuses = await dbContext
      .Bonuses.Include(v => v.Customer)
      .Where(v =>
        v.Customer.CompanyId == companyAccess.CompanyId
        && v.Status != Bonuses.BonusStatus.Cancelled
        && v.CreatedAt >= utcFrom
        && v.CreatedAt <= utcTo
      )
      .ToListAsync();

    if (shop is { })
    {
      bonuses = bonuses.Where(v => v.ShopId == shop.Id).ToList();
    }

    var customersCount = bonuses.DistinctBy(v => v.CustomerId).Count();
    var cashIncome = bonuses.Sum(v => v.CashIncome);
    var rawCashIncome = bonuses.Sum(v => v.RawCashIncome);

    var cashBonuses = bonuses
      .Where(v => v.Type == Bonuses.BonusType.AddBonus || v.Type == Bonuses.BonusType.SubtractBonus)
      .ToList();

    var operationsCount = cashBonuses.Count;

    decimal averageCheck =
      cashBonuses.Count > 0
        ? cashBonuses.Sum(v => v.CashIncome) / cashBonuses.Count
        : cashBonuses.Sum(v => v.CashIncome);

    var cashIncomeValues = cashBonuses
      .Select(v =>
      {
        var tzDateTimeOffset = new DateTimeOffset(
          v.CreatedAt.Year,
          v.CreatedAt.Month,
          v.CreatedAt.Day,
          v.CreatedAt.Hour,
          v.CreatedAt.Minute,
          v.CreatedAt.Second,
          tz!.BaseUtcOffset
        );

        return new GetGeneralAnalyticsResponse.CashIncomeValue(
          Date: new DateOnly(tzDateTimeOffset.Year, tzDateTimeOffset.Month, tzDateTimeOffset.Day),
          CashIncome: v.CashIncome,
          RawCashIncome: v.RawCashIncome
        );
      })
      .ToList()
      .GroupBy(v => v.Date)
      .ToDictionary(
        v => v.Key,
        v =>
          v.ToList()
            .Aggregate(
              (acc, vv) =>
                new GetGeneralAnalyticsResponse.CashIncomeValue(
                  Date: acc.Date,
                  CashIncome: acc.CashIncome + vv.CashIncome,
                  RawCashIncome: acc.RawCashIncome + vv.RawCashIncome
                )
            )
      );

    var data = new GetGeneralAnalyticsResponse.OkData(
      CashIncome: cashIncome,
      RawCashIncome: rawCashIncome,
      AverageCheck: averageCheck,
      OperationsCount: (uint)operationsCount,
      CustomersCount: (uint)customersCount,
      CashIncomeValues: cashIncomeValues.Values.ToList()
    );

    return new GetGeneralAnalyticsResponse.Ok(data);
  }
}
