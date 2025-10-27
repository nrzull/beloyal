namespace Beloyal.API.Web.Customers.CreateBooster;

public record CreateBoosterRequest(decimal CashbackPercent, DateTimeOffset ExpiredAt);
