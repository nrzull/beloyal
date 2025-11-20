using System.Security.Claims;
using System.Text;
using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Shops;
using Beloyal.API.Types;
using Beloyal.API.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Beloyal.API.Auth;

public class AuthContext(ApplicationDbContext dbContext)
{
  public static string GetAccessTokenName() => "beloyal_access_token";

  public static string GetRefreshTokenName() => "beloyal_refresh_token";

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
    var handler = new JsonWebTokenHandler();
    var expiredAt = DateTime.UtcNow.AddHours(12);
    var issuedAt = DateTime.UtcNow;

    var claims = new List<Claim>() { new("customer_id", customer.Id.ToString()) };

    var newDescriptor = (string t) =>
      new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = expiredAt,
        IssuedAt = issuedAt,
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(Encoding.UTF8.GetBytes(t)),
          SecurityAlgorithms.HmacSha256
        ),
      };

    var authToken = handler.CreateToken(newDescriptor(GetTokenSecret()!));
    var jwtAuthToken = handler.ReadJsonWebToken(authToken);

    var publicToken = new JsonWebTokenHandler().CreateToken(
      newDescriptor(GetCustomerPublicSecret()!)
    );
    var jwtPublicToken = handler.ReadJsonWebToken(publicToken);

    return (
      new EncodedAccessToken(authToken, new DateTimeOffset(jwtAuthToken.ValidFrom, TimeSpan.Zero)),
      new EncodedAccessToken(publicToken, new DateTimeOffset(jwtPublicToken.ValidTo, TimeSpan.Zero))
    );
  }

  public EncodedAccessToken EncodeAccessToken(
    UserId userId,
    CompanyAccessId? companyAccessId,
    ShopAccessId? shopAccessId
  )
  {
    var claims = new List<Claim> { new("user_id", userId.Value.ToString()) };
    if (companyAccessId?.Value is { })
    {
      claims.Add(new("company_access_id", companyAccessId.Value.ToString()));
    }
    if (shopAccessId?.Value is { })
    {
      claims.Add(new("shop_access_id", shopAccessId.Value.ToString()));
    }

    var handler = new JsonWebTokenHandler();
    var token = handler.CreateToken(
      new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddMinutes(5),
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetTokenSecret()!)),
          SecurityAlgorithms.HmacSha256
        ),
      }
    );

    var jwt = handler.ReadJsonWebToken(token);

    return new EncodedAccessToken(
      Token: token,
      ExpiredAt: new DateTimeOffset(jwt.ValidTo, TimeSpan.Zero)
    );
  }

  public async Task<CustomerAccessToken> DecodeCustomerToken(string token, string secretToken)
  {
    var handler = new JsonWebTokenHandler();

    var result = await handler.ValidateTokenAsync(
      token,
      new()
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretToken)),
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
      }
    );
    if (!result.IsValid)
    {
      throw result.Exception;
    }

    var jwt = (JsonWebToken)result.SecurityToken;
    var expiredAt = new DateTimeOffset(jwt.ValidTo, TimeSpan.Zero);
    var issuedAt = new DateTimeOffset(jwt.IssuedAt, TimeSpan.Zero);

    Guid customerId =
      jwt.TryGetClaim("customer_id", out var _customerId)
      && _customerId.Value is string { Length: > 0 }
        ? Guid.Parse(_customerId.Value)
        : throw new Exception("invalid customer_id");

    return new CustomerAccessToken(
      CustomerId: customerId,
      ExpiredAt: expiredAt,
      IssuedAt: issuedAt
    );
  }

  public async Task<CustomerAccessToken> DecodeCustomerPublicAccessToken(string token) =>
    await DecodeCustomerToken(token, GetCustomerPublicSecret()!);

  public async Task<CustomerAccessToken> DecodeCustomerAccessToken(string token) =>
    await DecodeCustomerToken(token, GetTokenSecret()!);

  public abstract record DecodeAccessTokenResult
  {
    public record Ok(AccessToken AccessToken) : DecodeAccessTokenResult;

    public record Expired : DecodeAccessTokenResult;

    public record Error(Exception Exception) : DecodeAccessTokenResult;
  }

  public async Task<DecodeAccessTokenResult> DecodeAccessToken(
    string token,
    bool validateLifetime = true
  )
  {
    try
    {
      var handler = new JsonWebTokenHandler();
      var result = await handler.ValidateTokenAsync(
        token,
        new()
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetTokenSecret()!)),
          ValidateAudience = false,
          ValidateIssuer = false,
          ValidateLifetime = validateLifetime,
          ClockSkew = TimeSpan.Zero,
        }
      );
      if (!result.IsValid)
      {
        throw result.Exception;
      }

      var jwt = (JsonWebToken)result.SecurityToken;
      var expiredAt = new DateTimeOffset(jwt.ValidTo, TimeSpan.Zero);

      Guid userId =
        jwt.TryGetClaim("user_id", out var _userId) && _userId.Value is string { Length: > 0 }
          ? Guid.Parse(_userId.Value)
          : throw new Exception("invalid user_id");

      Guid? companyAccessId =
        jwt.TryGetClaim("company_access_id", out var _companyAccessId)
        && _companyAccessId.Value is string { Length: > 0 }
          ? Guid.Parse(_companyAccessId.Value)
          : null;

      Guid? shopAccessId =
        jwt.TryGetClaim("shop_access_id", out var _shopAccessId)
        && _shopAccessId.Value is string { Length: > 0 }
          ? Guid.Parse(_shopAccessId.Value)
          : null;

      var accessToken = new AccessToken(
        UserId: userId,
        CompanyAccessId: companyAccessId,
        ShopAccessId: shopAccessId,
        ExpiredAt: expiredAt
      );

      return new DecodeAccessTokenResult.Ok(accessToken);
    }
    catch (SecurityTokenExpiredException)
    {
      return new DecodeAccessTokenResult.Expired();
    }
    catch (Exception e)
    {
      return new DecodeAccessTokenResult.Error(e);
    }
  }

  public async Task<bool> IsValidPersistedToken(UserId userId, string token) =>
    await dbContext.PersistedTokens.AnyAsync(v => v.UserId == userId.Value && v.Hash == token);

  public async Task<PersistedToken> EnsurePersistedToken(UserId userId, string token)
  {
    var persistedToken = await dbContext.PersistedTokens.SingleOrDefaultAsync(v =>
      v.UserId == userId.Value
    );

    if (persistedToken is null)
    {
      persistedToken = new PersistedToken
      {
        UserId = userId.Value,
        Hash = token,
        IssuedAt = DateTimeOffset.UtcNow,
      };
      dbContext.Add(persistedToken);
    }
    else
    {
      dbContext.Update(persistedToken);
      persistedToken.Hash = token;
      persistedToken.IssuedAt = DateTimeOffset.UtcNow;
    }

    await dbContext.SaveChangesAsync();

    return persistedToken;
  }

  public async Task<EncodedRefreshToken> EncodeRefreshToken(UserId userId)
  {
    var claims = new List<Claim> { new("user_id", userId.Value.ToString()) };

    var handler = new JsonWebTokenHandler();
    var token = handler.CreateToken(
      new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddMonths(1),
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetTokenSecret()!)),
          SecurityAlgorithms.HmacSha256
        ),
      }
    );

    var jwt = handler.ReadJsonWebToken(token);
    await EnsurePersistedToken(userId, token);

    return new EncodedRefreshToken(token, new DateTimeOffset(jwt.ValidTo, TimeSpan.Zero));
  }

  public async Task<RefreshToken> DecodeRefreshToken(string token)
  {
    var handler = new JsonWebTokenHandler();
    var result = await handler.ValidateTokenAsync(
      token,
      new()
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetTokenSecret()!)),
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
      }
    );
    if (!result.IsValid)
    {
      throw result.Exception;
    }

    var jwt = (JsonWebToken)result.SecurityToken;
    var expiredAt = new DateTimeOffset(jwt.ValidTo, TimeSpan.Zero);

    Guid userId =
      jwt.TryGetClaim("user_id", out var _userId) && _userId.Value is string { Length: > 0 }
        ? Guid.Parse(_userId.Value)
        : throw new Exception("invalid user_id");

    return new RefreshToken(UserId: userId, ExpiredAt: expiredAt);
  }

  public async Task<(EncodedAccessToken, EncodedRefreshToken)> IssueTokens(
    UserId userId,
    CompanyAccessId? companyAccessId,
    ShopAccessId? shopAccessId
  )
  {
    var accessToken = EncodeAccessToken(userId, companyAccessId, shopAccessId);
    var refreshToken = await EncodeRefreshToken(userId);
    return (accessToken, refreshToken);
  }

  public async Task<VerifyTokensResult> VerifyTokens(
    string rawAccessToken,
    string rawRefreshToken
  ) =>
    await DecodeAccessToken(rawAccessToken) switch
    {
      DecodeAccessTokenResult.Ok ok => new VerifyTokensResult.Ok(ok.AccessToken),
      DecodeAccessTokenResult.Error error => throw error.Exception,
      DecodeAccessTokenResult.Expired => await DecodeAccessToken(rawAccessToken, false) switch
      {
        DecodeAccessTokenResult.Error error => throw error.Exception,
        DecodeAccessTokenResult.Ok ok => await new Func<Task<VerifyTokensResult>>(async () =>
        {
          var refreshToken = await DecodeRefreshToken(rawRefreshToken);
          if (!await IsValidPersistedToken(UserId.From(refreshToken.UserId), rawRefreshToken))
          {
            throw new Exception("invalid refresh token");
          }

          var newTokens = await IssueTokens(
            UserId.From(refreshToken.UserId),
            ok.AccessToken.CompanyAccessId is { } cid ? CompanyAccessId.From(cid) : null,
            ok.AccessToken.ShopAccessId is { } sid ? ShopAccessId.From(sid) : null
          );

          return new VerifyTokensResult.Refresh(ok.AccessToken, newTokens.Item1, newTokens.Item2);
        })(),
        _ => throw new Exception("inner verify tokens failed"),
      },
      _ => throw new Exception("outer verify tokens failed"),
    };

  string? GetCustomerPublicSecret() =>
    Environment.GetEnvironmentVariable("API_AUTH_CUSTOMER_PUBLIC_SECRET");

  string? GetTokenSecret() => Environment.GetEnvironmentVariable("API_AUTH_SECRET");

  string GenCode() =>
    string.Join("", Enumerable.Range(1, 4).Select(_ => Random.Shared.NextInt64(1, 9)));
}

public abstract record VerifyTokensResult
{
  public record Ok(AccessToken AccessToken) : VerifyTokensResult;

  public record Refresh(
    AccessToken AccessToken,
    EncodedAccessToken EncodedAccessToken,
    EncodedRefreshToken EncodedRefreshToken
  ) : VerifyTokensResult;
}
