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

        var (chat, frame) = await chatContext.EnsureConversation(customer);

        Assert.Equal(customer.Id, chat.CustomerId);
        Assert.Equal(company.Id, chat.CompanyId);
        Assert.Equal(ChatFrameStatus.Opened, frame.Status);
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

        var msg = await chatContext.SendMessage(customer, "hello?");
        var (chat, frame) = await chatContext.EnsureConversation(customer);

        Assert.Equal(frame.Id, msg.FrameId);
        Assert.Equal(customer.Id, msg.CustomerId);
        Assert.Equal("hello?", msg.Content);
      }
    );

  [Fact]
  public Task ResolveConversation() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var (customer, _) = await CustomerFixture.Customer(scope, company);
        var chatContext = scope.Get<ChatContext>();
        var msg1 = await chatContext.SendMessage(customer, "hello?");
        var (_, frame1) = await chatContext.EnsureConversation(customer);
        await chatContext.ResolveConversation(customer, 5, "all good");

        Assert.Equal(ChatFrameStatus.Resolved, frame1.Status);

        var msg2 = await chatContext.SendMessage(customer, "hello again?");
        var (_, frame2) = await chatContext.EnsureConversation(customer);
        Assert.Equal(frame2.Id, msg2.FrameId);
        Assert.Equal(ChatFrameStatus.Opened, frame2.Status);
      }
    );

  [Fact]
  public Task GetMessages() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var (customer, _) = await CustomerFixture.Customer(scope, company);
        var chatContext = scope.Get<ChatContext>();

        var msg1 = await chatContext.SendMessage(customer, "hello?");
        var msg2 = await chatContext.SendMessage(customer, "hello world?");
        await chatContext.ResolveConversation(customer);

        var msg3 = await chatContext.SendMessage(customer, "hello again?");
        await chatContext.ResolveConversation(customer);

        var chat = await chatContext.GetChat(CustomerId.From(customer));

        var messages = await chatContext.GetMessages(ChatId.From(chat!));

        Assert.Equal(
          new List<Guid>() { msg3.Id, msg2.Id, msg1.Id },
          messages.Select(v => v.Id).ToList()
        );
      }
    );
}
