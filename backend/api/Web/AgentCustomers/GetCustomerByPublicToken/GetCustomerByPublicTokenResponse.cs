namespace Beloyal.API.Web.AgentCustomers.GetCustomerByPublicToken;

public abstract record GetCustomerByPublicTokenResponse
{
  public record Ok(OkData Data) : GetCustomerByPublicTokenResponse;

  public record OkData(
    decimal BonusAmount,
    decimal BonusMultiplier,
    decimal LevelPointsAmount,
    string PhoneNumber,
    Guid CustomerId
  );
}
