using Beloyal.API.Companies;
using Beloyal.API.Mailings;

namespace Beloyal.API.Web.Mailings.GetMailings;

public class GetMailingsEndpoint(MailingContext mailingContext)
{
  public async Task<GetMailingsResponse> Handle(CompanyAccess companyAccess)
  {
    var mailings = await mailingContext.GetMailings(CompanyId.From(companyAccess));

    return new GetMailingsResponse.Ok(mailings.Select(MailingDTO.From).ToList());
  }
}
