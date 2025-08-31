using Beloyal.API.Companies;
using Beloyal.API.Mailings;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Mailings.CreateMailing;

public class CreateMailingEndpoint(
  ApplicationDbContext dbContext,
  MailingContext mailingContext,
  CompanyContext companyContext
)
{
  public async Task<CreateMailingResponse> Handle(
    CompanyAccess companyAccess,
    CreateMailingRequest req
  )
  {
    var company = await companyContext.GetCompany(CompanyId.From(companyAccess));

    await using var tc = await dbContext.MaybeTransactionAsync();

    var mailing = await mailingContext.CreateMailing(
      company,
      req.Text,
      req.WithShops,
      req.ShopIds.Select(v => ShopId.From(v)).ToList()
    );

    await tc.CommitAsync();

    await Parallel.ForEachAsync(
      mailing.Units,
      async (unit, _cts) =>
      {
        try
        {
          await mailingContext.StartUnit(unit);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
      }
    );

    return new CreateMailingResponse.Ok(MailingDTO.From(mailing));
  }
}
