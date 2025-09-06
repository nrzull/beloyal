using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using Beloyal.API.Auth;
using Beloyal.API.Customers;
using Beloyal.API.Messengers;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Auth.LoginAsTgCustomer;

public class LoginAsTgCustomerEndpoint(
  TgContext tgContext,
  UserContext userContext,
  CustomerContext customerContext,
  AuthContext authContext
)
{
  public async Task<LoginAsTgCustomerResponse> Handle(LoginAsTgCustomerRequest req)
  {
    var bot = await tgContext.GetBot(TgBotId.From(req.TgBotId));
    if (bot is null)
    {
      throw new Exception("invalid tg bot");
    }

    var (result, tgUserData) = ValidateWebAppData(bot.Token, req.Hash, req.Query);

    if (
      result
      && tgUserData is { id: var tgUserId }
      && await userContext.GetByTgUserId(TgUserId.From(tgUserId)) is { } user
      && await customerContext.GetCustomer(CompanyId.From(bot.CompanyId), UserId.From(user))
        is { } customer
    )
    {
      var customerAccessToken = await authContext.EncodeCustomerAccessToken(customer);

      return new LoginAsTgCustomerResponse.Ok(customerAccessToken);
    }
    else
    {
      return new LoginAsTgCustomerResponse.InvalidUser();
    }
  }

  (bool, TgUserData?) ValidateWebAppData(
    string botToken,
    string telegramHash,
    string dataCheckString
  )
  {
    byte[] secretKeyBytes = Encoding.UTF8.GetBytes("WebAppData");
    byte[] botTokenBytes = Encoding.UTF8.GetBytes(botToken);

    byte[] secretKey;
    using (var hmacsha256 = new HMACSHA256(secretKeyBytes))
    {
      secretKey = hmacsha256.ComputeHash(botTokenBytes);
    }

    var queryParams = HttpUtility.ParseQueryString(dataCheckString);
    var sortedParams = new List<string>();

    foreach (string key in queryParams.AllKeys.OrderBy(k => k)!)
    {
      if (key != "hash")
      {
        sortedParams.Add($"{key}={queryParams[key]}");
      }
    }

    string dataToHash = string.Join("\n", sortedParams);
    byte[] dataToHashBytes = Encoding.UTF8.GetBytes(dataToHash);

    byte[] computedHash;
    using (var hmacsha256 = new HMACSHA256(secretKey))
    {
      computedHash = hmacsha256.ComputeHash(dataToHashBytes);
    }

    string computedHashHex = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

    var result = string.Equals(computedHashHex, telegramHash, StringComparison.OrdinalIgnoreCase);

    return (result, result ? GetTgUserData(dataCheckString) : null);
  }

  TgUserData? GetTgUserData(string dataCheckString)
  {
    var queryParams = HttpUtility.ParseQueryString(dataCheckString);
    if (queryParams["user"] != null)
    {
      try
      {
        string userJsonString = HttpUtility.UrlDecode(queryParams["user"]!);

        var user = JsonSerializer.Deserialize<TgUserData>(
          userJsonString,
          JsonSerializerOptions.Web
        );

        return user;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error deserializing user data: {ex.Message}");
        return null;
      }
    }

    return null;
  }

  class TgUserData
  {
    public long id { get; set; }
  }
}
