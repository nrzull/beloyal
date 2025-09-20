using Beloyal.API.Companies;

namespace Beloyal.API.Web.Company.GetCashback;

public abstract record GetCashbackResponse
{
  public record Ok(CompanyCashbackDTO Data) : GetCashbackResponse;
}
