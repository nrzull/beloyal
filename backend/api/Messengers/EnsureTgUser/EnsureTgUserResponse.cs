using Beloyal.API.Customers;
using Telegram.Bot;

namespace Beloyal.API.Messengers.EnsureTgUser;

public abstract record EnsureTgUserResponse
{
  public record InvalidMessage : EnsureTgUserResponse;

  public record InvalidTgBot : EnsureTgUserResponse;

  public record InvalidCustomer : EnsureTgUserResponse;

  public record RequestedPhoneNumber : EnsureTgUserResponse;

  public record Ok(TelegramBotClient Client, Customer Customer) : EnsureTgUserResponse;
}
