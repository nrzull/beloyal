using Beloyal.API.Auth;
using Beloyal.API.Types;

namespace Beloyal.API.Test.Auth;

public class AuthContextTest : TestLifetime
{
  [Fact]
  public Task CreateEmailOTP() =>
    WithScope(
      async (scope, _) =>
      {
        var authContext = scope.Get<AuthContext>();
        Email email = new("qwe@gma.com");
        var otp = await authContext.CreateEmailOTP(email);
        Assert.Equal(email.Value, otp.Email);
      }
    );

  [Fact]
  public Task DrainEmailOTP() =>
    WithScope(
      async (scope, _) =>
      {
        var authContext = scope.Get<AuthContext>();
        Email email = new("qwe@gma.com");

        var otp = await authContext.CreateEmailOTP(email);
        var otp2 = await authContext.CreateEmailOTP(email);

        Assert.Null(await authContext.DrainEmailOTP(email, otp.Code));

        Assert.IsType<AuthEmailOTP>(await authContext.DrainEmailOTP(email, otp2.Code));

        Assert.Null(await authContext.DrainEmailOTP(email, otp2.Code));
      }
    );
}
