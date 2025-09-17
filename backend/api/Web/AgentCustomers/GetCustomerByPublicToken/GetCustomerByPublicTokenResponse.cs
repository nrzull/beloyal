namespace Beloyal.API.Web.AgentCustomers.GetCustomerByPublicToken;

public abstract record GetCustomerByPublicTokenResponse
{
  public record Ok(OkData Data) : GetCustomerByPublicTokenResponse;

  public record OkData(
    decimal BonusAmount,
    decimal BonusMultiplier,
    string PhoneNumber,
    Guid CustomerId
  );
}
