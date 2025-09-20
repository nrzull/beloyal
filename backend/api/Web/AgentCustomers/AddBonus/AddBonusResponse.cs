using Beloyal.API.Bonuses;

namespace Beloyal.API.Web.AgentCustomers.AddBonus;

public abstract record AddBonusResponse
{
  public record Ok(BonusDTO Data) : AddBonusResponse;
}
