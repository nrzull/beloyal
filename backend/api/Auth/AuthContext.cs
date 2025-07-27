using Beloyal.API.Companies;
using Beloyal.API.Types;
using Beloyal.API.Users;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Auth;

public class AuthContext(ApplicationDbContext dbContext)
{
  public static string GetAccessTokenName() => "beloyal_access_token";

  public async Task<AuthEmailOTP> CreateEmailOTP(Email email)
  {
    await dbContext.AuthEmailOTPs.Where(v => v.Email == email.Value).ExecuteDeleteAsync();

    var emailOtp = new AuthEmailOTP()
    {
      Email = email.Value,
      Code = GenCode(),
      ExpiredAt = DateTimeOffset.UtcNow.AddMinutes(5),
    };

    dbContext.Add(emailOtp);
    await dbContext.SaveChangesAsync();

    return emailOtp;
  }

  public async Task<AuthEmailOTP?> DrainEmailOTP(Email email, string code)
  {
    var existing = await dbContext
      .AuthEmailOTPs.Where(v =>
        v.Code == code && v.Email == email.Value && v.ExpiredAt > DateTimeOffset.UtcNow
      )
      .FirstOrDefaultAsync();

    if (existing is null)
    {
      return null;
    }

    dbContext.Remove(existing);
    await dbContext.SaveChangesAsync();

    return existing;
  }

  public EncodedAccessToken EncodeAccessToken(User user, CompanyAccess? companyAccess)
  {
    var expiredAt = DateTimeOffset.UtcNow.AddHours(12);

    var token = JwtBuilder
      .Create()
      .WithAlgorithm(new HMACSHA256Algorithm())
      .WithSecret(GetTokenSecret())
      .AddClaim("exp", expiredAt.ToUnixTimeSeconds())
      .AddClaim("user_id", user.Id)
      .AddClaim("company_access_id", companyAccess?.Id)
      .Encode();

    return new EncodedAccessToken(Token: token, ExpiredAt: expiredAt);
  }

  public AccessToken DecodeAccessToken(string token)
  {
    var decoded = JwtBuilder
      .Create()
      .WithAlgorithm(new HMACSHA256Algorithm())
      .WithSecret(GetTokenSecret())
      .MustVerifySignature()
      .Decode<IDictionary<string, System.Text.Json.JsonElement>>(token);

    decoded.TryGetValue("exp", out var rawExpiredAtSeconds);
    decoded.TryGetValue("user_id", out var rawUserId);
    decoded.TryGetValue("company_access_id", out var rawCompanyAccessId);

    if (!rawExpiredAtSeconds.TryGetInt64(out long expiredAtSeconds))
    {
      throw new Exception("exp is invalid");
    }

    if (!rawUserId.TryGetGuid(out var userId))
    {
      throw new Exception("user_id is invalid");
    }

    return new AccessToken(
      UserId: userId,
      CompanyAccessId: rawCompanyAccessId.ValueKind == System.Text.Json.JsonValueKind.String
        ? rawCompanyAccessId.GetGuid()
        : null,
      ExpiredAt: DateTimeOffset.FromUnixTimeSeconds(expiredAtSeconds)
    );
  }

  string? GetTokenSecret() => Environment.GetEnvironmentVariable("API_AUTH_SECRET");

  string GenCode() =>
    string.Join("", Enumerable.Range(1, 8).Select(_ => Random.Shared.NextInt64(1, 9)));
}
