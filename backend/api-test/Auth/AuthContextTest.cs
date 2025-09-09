using Beloyal.API.Auth;
using Beloyal.API.Types;

namespace Beloyal.API.Test.Auth;

public class AuthContextTest : TestLifetime
{
  [Fact]
  public Task CreateOtp() =>
    WithScope(
      async (scope, _) =>
      {
        var authContext = scope.Get<AuthContext>();
        Phone phone = new("+123123123123");
        var otp = await authContext.CreateOtp(phone);
        Assert.Equal(phone.Value, otp.Phone);
      }
    );

  [Fact]
  public Task DrainOtp() =>
    WithScope(
      async (scope, _) =>
      {
        var authContext = scope.Get<AuthContext>();
        Phone phone = new("+123123123123");

        var otp = await authContext.CreateOtp(phone);
        var otp2 = await authContext.CreateOtp(phone);

        Assert.Null(await authContext.DrainOtp(phone, otp.Code));

        Assert.IsType<AuthOtp>(await authContext.DrainOtp(phone, otp2.Code));

        Assert.Null(await authContext.DrainOtp(phone, otp2.Code));
      }
    );
}
