using Beloyal.API.Mailings;

namespace Beloyal.API.Web.Mailings.CreateMailing;

public abstract record CreateMailingResponse
{
  public record Ok(MailingDTO Data) : CreateMailingResponse;
}
