namespace Beloyal.API.Web.Auth.LoginAsTgCustomer;

public record LoginAsTgCustomerRequest(string Query, string Hash, Guid TgBotId);
