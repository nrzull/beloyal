using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Chats;

public class ChatContext(ApplicationDbContext dbContext)
{
  public async Task<Chat?> GetChat(CustomerId customerId) =>
    await dbContext.Chats.FirstOrDefaultAsync(v => v.CustomerId == customerId.Value);

  public async Task MarkAsRead(ChatMessageId id, CompanyAccess reader)
  {
    var msg = await dbContext.ChatMessages.FirstOrDefaultAsync(v =>
      v.Id == id.Value && v.AgentId != reader.Id && v.ReadAt == null
    );

    if (msg is null)
    {
      return;
    }

    dbContext.Update(msg);
    msg.ReadAt = DateTimeOffset.UtcNow;

    await dbContext.SaveChangesAsync();
  }

  public async Task MarkAsRead(ChatMessageId id, Customer reader)
  {
    var msg = await dbContext.ChatMessages.FirstOrDefaultAsync(v =>
      v.Id == id.Value && v.CustomerId != reader.Id && v.ReadAt == null
    );

    if (msg is null)
    {
      return;
    }

    dbContext.Update(msg);
    msg.ReadAt = DateTimeOffset.UtcNow;

    await dbContext.SaveChangesAsync();
  }

  public async Task<List<Chat>> GetChats(CompanyId companyId)
  {
    var chats = await dbContext
      .Chats.Where(v => v.CompanyId == companyId.Value)
      .Include(v => v.Messages.OrderByDescending(vv => vv.CreatedAt).Take(1))
      .ThenInclude(v => v.Customer)
      .ThenInclude(v => v!.User)
      .Include(v => v.Messages)
      .ThenInclude(v => v.Agent)
      .ThenInclude(v => v!.User)
      .ToListAsync();

    return chats.OrderDescending(new MessageCreatedAtComparer()).ToList();
  }

  public async Task<Chat> EnsureConversation(Customer customer)
  {
    var chat = await GetChat(CustomerId.From(customer));
    if (chat is null)
    {
      await dbContext.Entry(customer).Reference(v => v.Company).LoadAsync();
      chat = new Chat { Customer = customer, Company = customer.Company };
      dbContext.Add(chat);
      await dbContext.SaveChangesAsync();
    }

    return chat;
  }

  public async Task<ChatMessage> SendMessage(Customer customer, string content)
  {
    var chat = await EnsureConversation(customer);

    var message = new ChatMessage
    {
      Content = content,
      Customer = customer,
      Chat = chat,
    };
    dbContext.Add(message);
    await dbContext.SaveChangesAsync();

    return message;
  }

  public async Task<ChatMessage> SendMessage(CompanyAccess agent, Customer customer, string content)
  {
    var chat = await EnsureConversation(customer);

    var message = new ChatMessage
    {
      Content = content,
      Agent = agent,
      Chat = chat,
    };
    dbContext.Add(message);
    await dbContext.SaveChangesAsync();

    return message;
  }

  public async Task<List<ChatMessage>> GetChatMessages(CompanyId companyId, ChatId chatId) =>
    await dbContext
      .ChatMessages.Include(v => v.Chat)
      .Where(v => v.ChatId == chatId.Value && v.Chat.CompanyId == companyId.Value)
      .Include(v => v.Agent)
      .ThenInclude(v => v!.User)
      .Include(v => v.Customer)
      .ThenInclude(v => v!.User)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();
}

class MessageCreatedAtComparer : IComparer<Chat>
{
  public int Compare(Chat? x, Chat? y)
  {
    var first = x?.Messages.OrderByDescending(vv => vv.CreatedAt)?.FirstOrDefault();

    var second = y?.Messages.OrderByDescending(vv => vv.CreatedAt)?.FirstOrDefault();

    return first?.CreatedAt < second?.CreatedAt ? -1
      : second?.CreatedAt < first?.CreatedAt ? 1
      : 0;
  }
}
