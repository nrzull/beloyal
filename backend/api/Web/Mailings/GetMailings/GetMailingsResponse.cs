using Beloyal.API.Mailings;

namespace Beloyal.API.Web.Mailings.GetMailings;

public abstract record GetMailingsResponse
{
  public record Ok(List<MailingDTO> Data) : GetMailingsResponse;
}
