using Beloyal.API.Boosters;

namespace Beloyal.API.Web.Customers.GetCustomerBoosters;

public abstract record GetCustomerBoostersResponse
{
  public record Ok(List<BoosterDTO> Data) : GetCustomerBoostersResponse;
}
