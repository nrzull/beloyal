using Amazon.S3;
using Amazon.S3.Model;
using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Mailings;
using Beloyal.API.Messengers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Beloyal.API.Chats;

public class ChatContext(
  ApplicationDbContext dbContext,
  IHubContext<ApplicationHub> appHub,
  TgContext tgContext,
  AmazonS3Client s3Client
)
{
  public async Task<Chat?> GetChat(CustomerId customerId) =>
    await dbContext.Chats.FirstOrDefaultAsync(v => v.CustomerId == customerId.Value);

  public async Task<Chat?> GetChat(CompanyId companyId, ChatId chatId) =>
    await dbContext
      .Chats.Include(v => v.Customer)
      .FirstOrDefaultAsync(v => v.Id == chatId.Value && v.CompanyId == companyId.Value);

  public async Task<Chat?> GetChatWithLastMessage(
    CompanyId companyId,
    ChatId chatId,
    bool withSystem
  ) =>
    await dbContext
      .Chats.Include(v =>
        withSystem
          ? v.Messages.OrderByDescending(vv => vv.CreatedAt).Take(1)
          : v
            .Messages.OrderByDescending(vv => vv.CreatedAt)
            .Where(vv => vv.AgentId != null || vv.CustomerId != null)
            .Take(1)
      )
      .ThenInclude(v => v.Customer)
      .ThenInclude(v => v!.User)
      .Include(v => v.Messages)
      .ThenInclude(v => v.Agent)
      .ThenInclude(v => v!.User)
      .Include(v => v.Messages)
      .ThenInclude(v => v.MailingUnit)
      .ThenInclude(v => v!.Mailing)
      .Include(v => v.Customer)
      .ThenInclude(v => v.User)
      .AsSplitQuery()
      .FirstOrDefaultAsync(v => v.CompanyId == companyId.Value && v.Id == chatId.Value);

  public async Task<List<Chat>> GetChats(CompanyId companyId, bool withSystem)
  {
    var chats = await dbContext
      .Chats.Where(v => v.CompanyId == companyId.Value)
      .Include(v =>
        withSystem
          ? v.Messages.OrderByDescending(vv => vv.CreatedAt).Take(1)
          : v
            .Messages.OrderByDescending(vv => vv.CreatedAt)
            .Where(vv => vv.AgentId != null || vv.CustomerId != null)
            .Take(1)
      )
      .ThenInclude(v => v.Customer)
      .ThenInclude(v => v!.User)
      .Include(v => v.Messages)
      .ThenInclude(v => v.Agent)
      .ThenInclude(v => v!.User)
      .Include(v => v.Messages)
      .ThenInclude(v => v.MailingUnit)
      .ThenInclude(v => v!.Mailing)
      .Include(v => v.Customer)
      .ThenInclude(v => v.User)
      .AsSplitQuery()
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

  public async Task SendVirtualMessage(Customer customer, string content)
  {
    var client = customer.TgBot is { }
      ? await tgContext.EnsureClient(CompanyId.From(customer), customer.TgBot)
      : null;

    if (client is null)
    {
      throw new Exception("no client");
    }

    await client.SendMessage(customer.User.TgUserId!, content);
  }

  public async Task SendMessage(
    ChatMessage msg,
    List<ChatFileStream>? attachments,
    bool toCustomer = true,
    bool closeFileStream = true
  )
  {
    await dbContext.Entry(msg).Reference(v => v.Chat).LoadAsync();
    await dbContext.Entry(msg.Chat).Reference(v => v.Customer).LoadAsync();
    var customer = msg.Chat.Customer;

    if (toCustomer)
    {
      await dbContext.Entry(customer).Reference(v => v.TgBot).LoadAsync();
      await dbContext.Entry(customer).Reference(v => v.MaxBot).LoadAsync();

      var client = customer.TgBot is { }
        ? await tgContext.EnsureClient(CompanyId.From(customer), customer.TgBot)
        : null;

      if (client is null)
      {
        throw new Exception("no client");
      }

      await dbContext.Entry(customer).Reference(v => v.User).LoadAsync();

      try
      {
        if (attachments is { Count: > 0 })
        {
          var imagesOnly = attachments.All(v => v.MimeType.StartsWith("image/"));

          List<IAlbumInputMedia> media = new();

          foreach (var (idx, file) in attachments.Index())
          {
            var inputFile = new InputFileStream(file.Stream, file.Title);

            media.Add(
              imagesOnly ? new InputMediaPhoto(inputFile) : new InputMediaDocument(inputFile)
            );
          }

          if (media.FirstOrDefault() is InputMediaPhoto l)
          {
            l.Caption = msg.Content!;
          }
          else if (media.LastOrDefault() is InputMediaDocument ll)
          {
            ll.Caption = msg.Content!;
          }

          await client.SendMediaGroup(customer.User.TgUserId!, media: media);
        }
        else
        {
          await client.SendMessage(customer.User.TgUserId!, msg.Content!);
        }
      }
      finally
      {
        if (closeFileStream)
        {
          foreach (var attachment in attachments ?? [])
          {
            await attachment.Stream.DisposeAsync();
          }
        }
      }
    }

    await appHub
      .Clients.Clients(ApplicationHub.GetCompanyConnections(CompanyId.From(customer)) ?? [])!
      .SendAsync("ChatMessage", ChatMessageDTO.From(msg, s3Client));
  }

  public async Task<ChatMessage> CreateMessage(
    Customer customer,
    string content,
    List<ChatFileStream>? attachments
  )
  {
    var chat = await EnsureConversation(customer);

    var message = new ChatMessage
    {
      Chat = chat,
      Customer = customer,
      Content = content,
    };
    dbContext.Add(message);
    await dbContext.SaveChangesAsync();
    await UploadAttachments(message, attachments);

    return message;
  }

  public async Task<ChatMessage> CreateMessage(Company company, Customer customer, string content)
  {
    var chat = await EnsureConversation(customer);

    var message = new ChatMessage
    {
      Chat = chat,
      Company = company,
      Content = content,
    };
    dbContext.Add(message);
    await dbContext.SaveChangesAsync();

    return message;
  }

  public async Task<ChatMessage> CreateMessage(MailingUnit unit, Customer customer, string content)
  {
    var chat = await EnsureConversation(customer);

    var message = new ChatMessage
    {
      Chat = chat,
      MailingUnit = unit,
      Content = content,
    };
    dbContext.Add(message);
    await dbContext.SaveChangesAsync();

    return message;
  }

  public async Task UploadAttachments(ChatMessage message, List<ChatFileStream>? attachments = null)
  {
    if (attachments is { Count: > 0 })
    {
      var bucketName = message.Id.ToString();

      await s3Client.PutBucketAsync(
        new PutBucketRequest() { BucketName = bucketName, UseClientRegion = false }
      );

      dbContext.Update(message);
      message.AttachmentKeys = [];
      foreach (var attachment in attachments)
      {
        var req = new PutObjectRequest()
        {
          Key = Guid.NewGuid().ToString(),
          BucketName = bucketName,
          ContentType = attachment.MimeType,
          InputStream = attachment.Stream,
        };

        await s3Client.PutObjectAsync(req);
        message.AttachmentKeys.Add(req.Key);
      }

      await dbContext.SaveChangesAsync();
    }
  }

  public async Task<ChatMessage> CreateMessage(
    CompanyAccess agent,
    Customer customer,
    string content,
    List<IFormFile>? attachments
  )
  {
    var chat = await EnsureConversation(customer);

    var message = new ChatMessage
    {
      Chat = chat,
      Agent = agent,
      Content = content,
    };

    dbContext.Add(message);
    await dbContext.SaveChangesAsync();
    await UploadAttachments(
      message,
      attachments
        ?.Select(v => new ChatFileStream(v.ContentType, v.FileName, v.OpenReadStream()))
        .ToList()
    );

    return message;
  }

  public async Task<List<ChatMessage>> GetChatMessages(
    CompanyId companyId,
    ChatId chatId,
    bool withSystem
  ) =>
    await dbContext
      .ChatMessages.Include(v => v.Chat)
      .Where(v =>
        withSystem
          ? v.ChatId == chatId.Value && v.Chat.CompanyId == companyId.Value
          : v.ChatId == chatId.Value
            && v.Chat.CompanyId == companyId.Value
            && (v.AgentId != null || v.CustomerId != null)
      )
      .Include(v => v.Agent)
      .ThenInclude(v => v!.User)
      .Include(v => v.Customer)
      .ThenInclude(v => v!.User)
      .Include(v => v.MailingUnit)
      .ThenInclude(v => v!.Mailing)
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
