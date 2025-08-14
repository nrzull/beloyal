using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Chats;

public class ChatContext(ApplicationDbContext dbContext)
{
  public async Task<Chat?> GetChat(CustomerId customerId) =>
    await dbContext.Chats.FirstOrDefaultAsync(v => v.CustomerId == customerId.Value);

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
    var (_, frame) = await EnsureConversation(customer);

    var message = new ChatMessage
    {
      Frame = frame,
      Content = content,
      Customer = customer,
    };
    dbContext.Add(message);
    await dbContext.SaveChangesAsync();

    return message;
  }

  public async Task<ChatMessage> SendMessage(CompanyAccess agent, Customer customer, string content)
  {
    var (_, frame) = await EnsureConversation(customer);

    var message = new ChatMessage
    {
      Frame = frame,
      Content = content,
      Agent = agent,
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
