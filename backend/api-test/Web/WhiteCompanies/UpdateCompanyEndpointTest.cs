using Beloyal.API.Companies;
using Beloyal.API.Web.WhiteCompanies.UpdateCompany;

namespace Beloyal.API.Test.Web.WhiteCompanies;

public class UpdateCompanyEndpointTest : TestLifetime
{
  [Fact]
  public Task Update() =>
    WithScope(
      async (scope, _) =>
      {
        var companyContext = scope.Get<CompanyContext>();

        var updated = DateTimeOffset.UtcNow.AddDays(1);

        var whiteCompany = await companyContext.CreateWhiteCompany(
          new("1234567891"),
          DateTimeOffset.UtcNow
        );

        var result = await scope.Get<UpdateCompanyEndpoint>().Handle(new(whiteCompany.Id, updated));

        Assert.IsType<UpdateCompanyResponse.Ok>(result);

        if (result is UpdateCompanyResponse.Ok ok)
        {
          Assert.Equal(updated, ok.Data.ExpiredAt);
        }
      }
    );
}
