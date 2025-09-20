using Beloyal.API.Companies;

namespace Beloyal.API.Web.Company.SetCashback;

public abstract record SetCashbackResponse
{
  public record Ok(CompanyCashbackDTO Data) : SetCashbackResponse;
}
