using Beloyal.API.Boosters;

namespace Beloyal.API.Web.Customers.CreateBooster;

public abstract record CreateBoosterResponse
{
  public record Ok(BoosterDTO Data) : CreateBoosterResponse;
}
