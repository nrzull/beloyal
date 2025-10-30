using Beloyal.API.Chats;
using Beloyal.API.Customers;
using Beloyal.API.Test.Companies;
using Beloyal.API.Test.Customers;

namespace Beloyal.API.Test.Chats;

public class ChatContextTest : TestLifetime
{
  [Fact]
  public Task EnsureConversation() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var (customer, _) = await CustomerFixture.Customer(scope, company);
        var chatContext = scope.Get<ChatContext>();

        var chat = await chatContext.EnsureConversation(customer);

        Assert.Equal(customer.Id, chat.CustomerId);
        Assert.Equal(company.Id, chat.CompanyId);
      }
    );

  [Fact]
  public Task SendMessage() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var (customer, _) = await CustomerFixture.Customer(scope, company);
        var chatContext = scope.Get<ChatContext>();

        var msg = await chatContext.CreateMessage(customer, "hello?", []);
        var chat = await chatContext.EnsureConversation(customer);

        Assert.Equal(customer.Id, msg.CustomerId);
        Assert.Equal("hello?", msg.Content);
      }
    );

  [Fact]
  public Task GetChatMessages() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var (customer, _) = await CustomerFixture.Customer(scope, company);
        var chatContext = scope.Get<ChatContext>();

        var msg1 = await chatContext.CreateMessage(customer, "hello?", []);
        var msg2 = await chatContext.CreateMessage(customer, "hello world?", []);
        var msg3 = await chatContext.CreateMessage(customer, "hello again?", []);

        var chat = await chatContext.GetChat(CustomerId.From(customer));
        var messages = await chatContext.GetChatMessages(
          CompanyId.From(company),
          ChatId.From(chat!),
          true
        );

        Assert.Equal(
          new List<Guid>() { msg3.Id, msg2.Id, msg1.Id },
          messages.Select(v => v.Id).ToList()
        );
      }
    );
}
