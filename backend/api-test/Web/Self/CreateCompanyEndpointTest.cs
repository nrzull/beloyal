using Beloyal.API.Companies;
using Beloyal.API.Test.Users;
using Beloyal.API.Types;
using Beloyal.API.Web.Self.CreateCompany;

namespace Beloyal.API.Test.Web.Self;

public class CreateCompanyEndpointTest : TestLifetime
{
  [Fact]
  public Task CreateCompany() =>
    WithScope(
      async (scope, _) =>
      {
        var endpoint = scope.Get<CreateCompanyEndpoint>();
        var companyContext = scope.Get<CompanyContext>();

        var user = await UserFixture.User(scope, "qweqwe@gma.com");
        var tin = new Tin("1234567891");

        await Assert.ThrowsAnyAsync<Exception>(() => endpoint.Handle(user, tin));

        await companyContext.CreateWhiteCompany(tin, DateTimeOffset.UtcNow.AddDays(1));

        Assert.IsType<CreateCompanyResponse.Ok>(await endpoint.Handle(user, tin));

        Assert.Single(await companyContext.GetCompanyAccesses(user));
      }
    );
}
