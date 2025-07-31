using Beloyal.API.Web.WhiteCompanies.AddCompany;

namespace Beloyal.API.Test.Web.WhiteCompanies;

public class AddCompanyEndpointTest : TestLifetime
{
  [Fact]
  public Task Create() =>
    WithScope(
      async (scope, _) =>
      {
        var endpoint = scope.Get<AddCompanyEndpoint>();

        var result = await endpoint.Handle(new("1234567891", DateTimeOffset.UtcNow.AddDays(1)));

        Assert.IsType<AddCompanyResponse.Ok>(result);
      }
    );
}
