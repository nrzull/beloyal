namespace Beloyal.API.Web.Analytics.GetGeneralAnalytics;

public abstract record GetGeneralAnalyticsResponse
{
  public sealed record Ok(OkData Data) : GetGeneralAnalyticsResponse;

  public record OkData(
    decimal CashIncome,
    decimal RawCashIncome,
    decimal AverageCheck,
    uint OperationsCount,
    uint CustomersCount,
    List<CashIncomeValue> CashIncomeValues
  );

  public record CashIncomeValue(DateOnly Date, decimal CashIncome, decimal RawCashIncome);
}
