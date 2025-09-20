using Beloyal.API.Bonuses;

namespace Beloyal.API.Web.AgentCustomers.SubtractBonus;

public abstract record SubtractBonusResponse
{
  public record Ok(BonusDTO Data) : SubtractBonusResponse;
}
