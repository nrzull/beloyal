using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Shops;
using Beloyal.API.Types;
using Beloyal.API.Users;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Auth;

public class AuthContext(ApplicationDbContext dbContext)
{
  public static string GetAccessTokenName() => "beloyal_access_token";

  public async Task<AuthOtp> CreateOtp(Phone phone)
  {
    await dbContext.AuthOtps.Where(v => v.Phone == phone.Value).ExecuteDeleteAsync();

    var otp = new AuthOtp()
    {
      Phone = phone.Value,
      Code = GenCode(),
      ExpiredAt = DateTimeOffset.UtcNow.AddMinutes(5),
    };

    dbContext.Add(otp);
    await dbContext.SaveChangesAsync();

    return otp;
  }

  public async Task<AuthOtp?> DrainOtp(Phone phone, string code)
  {
    var existing = await dbContext
      .AuthOtps.Where(v =>
        v.Code == code && v.Phone == phone.Value && v.ExpiredAt > DateTimeOffset.UtcNow
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

  public (EncodedAccessToken authToken, EncodedAccessToken publicToken) EncodeCustomerAccessToken(
    Customer customer
  )
  {
    var token = new CustomerAccessToken(
      CustomerId: customer.Id,
      ExpiredAt: DateTimeOffset.UtcNow.AddHours(1)
    );

    var authToken = JwtBuilder
      .Create()
      .WithAlgorithm(new HMACSHA256Algorithm())
      .WithSecret(GetTokenSecret())
      .AddClaim("exp", token.ExpiredAt.ToUnixTimeSeconds())
      .AddClaim("customer_id", token.CustomerId)
      .Encode();

    var publicToken = JwtBuilder
      .Create()
      .WithAlgorithm(new HMACSHA256Algorithm())
      .WithSecret(GetCustomerPublicSecret())
      .AddClaim("exp", token.ExpiredAt.ToUnixTimeSeconds())
      .AddClaim("customer_id", token.CustomerId)
      .Encode();

    return (
      new EncodedAccessToken(authToken, token.ExpiredAt),
      new EncodedAccessToken(publicToken, token.ExpiredAt)
    );
  }

  public EncodedAccessToken EncodeAccessToken(
    User user,
    CompanyAccess? companyAccess,
    ShopAccess? shopAccess
  )
  {
    var expiredAt = DateTimeOffset.UtcNow.AddHours(12);

    var token = JwtBuilder
      .Create()
      .WithAlgorithm(new HMACSHA256Algorithm())
      .WithSecret(GetTokenSecret())
      .AddClaim("exp", expiredAt.ToUnixTimeSeconds())
      .AddClaim("user_id", user.Id)
      .AddClaim("company_access_id", companyAccess?.Id)
      .AddClaim("shop_access_id", shopAccess?.Id)
      .Encode();

    return new EncodedAccessToken(Token: token, ExpiredAt: expiredAt);
  }

  public CustomerAccessToken DecodeCustomerPublicAccessToken(string token)
  {
    var decoded = JwtBuilder
      .Create()
      .WithAlgorithm(new HMACSHA256Algorithm())
      .WithSecret(GetCustomerPublicSecret())
      .MustVerifySignature()
      .Decode<IDictionary<string, System.Text.Json.JsonElement>>(token);

    decoded.TryGetValue("exp", out var _expiredAtSeconds);
    decoded.TryGetValue("customer_id", out var _customerId);

    if (!_expiredAtSeconds.TryGetInt64(out long expiredAtSeconds))
    {
      throw new Exception("exp is invalid");
    }

    if (!_customerId.TryGetGuid(out var customerId))
    {
      throw new Exception("customer_id is invalid");
    }

    return new CustomerAccessToken(
      CustomerId: customerId,
      ExpiredAt: DateTimeOffset.FromUnixTimeSeconds(expiredAtSeconds)
    );
  }

  public CustomerAccessToken DecodeCustomerAccessToken(string token)
  {
    var decoded = JwtBuilder
      .Create()
      .WithAlgorithm(new HMACSHA256Algorithm())
      .WithSecret(GetTokenSecret())
      .MustVerifySignature()
      .Decode<IDictionary<string, System.Text.Json.JsonElement>>(token);

    decoded.TryGetValue("exp", out var _expiredAtSeconds);
    decoded.TryGetValue("customer_id", out var _customerId);

    if (!_expiredAtSeconds.TryGetInt64(out long expiredAtSeconds))
    {
      throw new Exception("exp is invalid");
    }

    if (!_customerId.TryGetGuid(out var customerId))
    {
      throw new Exception("customer_id is invalid");
    }

    return new CustomerAccessToken(
      CustomerId: customerId,
      ExpiredAt: DateTimeOffset.FromUnixTimeSeconds(expiredAtSeconds)
    );
  }

  public AccessToken DecodeAccessToken(string token)
  {
    var decoded = JwtBuilder
      .Create()
      .WithAlgorithm(new HMACSHA256Algorithm())
      .WithSecret(GetTokenSecret())
      .MustVerifySignature()
      .Decode<IDictionary<string, System.Text.Json.JsonElement>>(token);

    decoded.TryGetValue("exp", out var _expiredAtSeconds);
    decoded.TryGetValue("user_id", out var _userId);
    decoded.TryGetValue("company_access_id", out var _companyAccessId);
    decoded.TryGetValue("shop_access_id", out var _shopAccessId);

    if (!_expiredAtSeconds.TryGetInt64(out long expiredAtSeconds))
    {
      throw new Exception("exp is invalid");
    }

    if (!_userId.TryGetGuid(out var userId))
    {
      throw new Exception("user_id is invalid");
    }

    return new AccessToken(
      UserId: userId,
      CompanyAccessId: _companyAccessId.ValueKind == System.Text.Json.JsonValueKind.String
        ? _companyAccessId.GetGuid()
        : null,
      ShopAccessId: _shopAccessId.ValueKind == System.Text.Json.JsonValueKind.String
        ? _shopAccessId.GetGuid()
        : null,
      ExpiredAt: DateTimeOffset.FromUnixTimeSeconds(expiredAtSeconds)
    );
  }

  string? GetCustomerPublicSecret() =>
    Environment.GetEnvironmentVariable("API_AUTH_CUSTOMER_PUBLIC_SECRET");

  string? GetTokenSecret() => Environment.GetEnvironmentVariable("API_AUTH_SECRET");

  string GenCode() =>
    string.Join("", Enumerable.Range(1, 8).Select(_ => Random.Shared.NextInt64(1, 9)));
}
