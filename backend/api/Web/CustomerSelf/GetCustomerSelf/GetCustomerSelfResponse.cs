namespace Beloyal.API.Web.CustomerSelf.GetCustomerSelf;

public abstract record GetCustomerSelfResponse
{
  public record Ok(OkData Data) : GetCustomerSelfResponse;

  public record OkData(string CompanyName, decimal BonusAmount, decimal BonusMultiplier);
}
