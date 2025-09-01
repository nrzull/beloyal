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

  public async Task<List<ChatMessage>> GetChatMessages(CompanyId companyId, ChatId chatId) =>
    await dbContext
      .ChatMessages.Include(v => v.Chat)
      .Include(v => v.Agent)
      .ThenInclude(v => v!.User)
      .Include(v => v.Customer)
      .ThenInclude(v => v!.User)
      .Where(v => v.ChatId == chatId.Value && v.Chat.CompanyId == companyId.Value)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task<List<Chat>> GetChats(CompanyId companyId)
  {
    List<Chat> chats = [];

    foreach (
      var chat in await dbContext.Chats.Where(v => v.CompanyId == companyId.Value).ToListAsync()
    )
    {
      var frame = await dbContext
        .ChatFrames.Where(v => v.ChatId == chat.Id)
        .OrderByDescending(v => v.CreatedAt)
        .FirstOrDefaultAsync();

      if (frame is { })
      {
        var msg = await dbContext
          .ChatMessages.Where(v => v.FrameId == frame.Id)
          .OrderByDescending(v => v.CreatedAt)
          .Include(v => v.Agent)
          .ThenInclude(v => v!.User)
          .Include(v => v.Customer)
          .ThenInclude(v => v!.User)
          .FirstOrDefaultAsync();

        if (msg is { })
        {
          frame.Messages = [msg];
          chat.Messages = [msg];
        }

        chat.Frames = [frame];
      }

      chats = [.. chats, chat];
    }

    return chats;
  }

  public async Task<ChatFrame?> GetConversationFrame(Chat chat) =>
    await dbContext.ChatFrames.FirstOrDefaultAsync(v =>
      v.Status == ChatFrameStatus.Opened
      && v.ChatId == chat.Id
      && v.Type == ChatFrameType.Conversation
    );

  public async Task<(Chat, ChatFrame)> EnsureConversation(Customer customer)
  {
    var chat = await GetChat(CustomerId.From(customer));
    if (chat is null)
    {
      await dbContext.Entry(customer).Reference(v => v.Company).LoadAsync();
      chat = new Chat { Customer = customer, Company = customer.Company };
      dbContext.Add(chat);
      await dbContext.SaveChangesAsync();
    }

    var frame = await GetConversationFrame(chat);
    if (frame is null)
    {
      frame = new ChatFrame
      {
        Chat = chat,
        Status = ChatFrameStatus.Opened,
        Type = ChatFrameType.Conversation,
      };
      dbContext.Add(frame);
      await dbContext.SaveChangesAsync();
    }

    return (chat, frame);
  }

  public async Task<ChatMessage> SendMessage(Customer customer, string content)
  {
    var (chat, frame) = await EnsureConversation(customer);

    var message = new ChatMessage
    {
      Frame = frame,
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
    var (chat, frame) = await EnsureConversation(customer);

    var message = new ChatMessage
    {
      Frame = frame,
      Content = content,
      Agent = agent,
      Chat = chat,
    };
    dbContext.Add(message);
    await dbContext.SaveChangesAsync();

    return message;
  }

  public async Task<List<ChatMessage>> GetMessages(ChatId chatId)
  {
    var frames = await dbContext
      .ChatFrames.Where(v => v.ChatId == chatId.Value)
      .Include(v => v.Messages)
      .ThenInclude(v => v.Customer)
      .Include(v => v.Messages)
      .ThenInclude(v => v.Agent)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

    return frames.SelectMany(v => v.Messages.OrderByDescending(v => v.CreatedAt)).ToList();
  }

  public async Task ResolveConversation(
    Customer customer,
    int? rate = null,
    string? rateComment = null
  )
  {
    var chat = await GetChat(CustomerId.From(customer));
    if (chat is null)
    {
      throw new Exception("invalid chat");
    }
    var frame = await GetConversationFrame(chat);
    if (frame is null)
    {
      throw new Exception("no active conversation");
    }

    dbContext.Update(frame);
    frame.Status = ChatFrameStatus.Resolved;
    frame.Rate = rate;
    frame.RateComment = rateComment;
    await dbContext.SaveChangesAsync();
  }
}

class MessageCreatedAtComparer : IComparer<Chat>
{
  public int Compare(Chat? x, Chat? y)
  {
    var first = x
      ?.Frames.OrderByDescending(v => v.CreatedAt)
      .FirstOrDefault()
      ?.Messages.OrderByDescending(vv => vv.CreatedAt)
      ?.FirstOrDefault();

    var second = y
      ?.Frames.OrderByDescending(v => v.CreatedAt)
      .FirstOrDefault()
      ?.Messages.OrderByDescending(vv => vv.CreatedAt)
      ?.FirstOrDefault();

    return first?.CreatedAt < second?.CreatedAt ? -1
      : second?.CreatedAt < first?.CreatedAt ? 1
      : 0;
  }
}
