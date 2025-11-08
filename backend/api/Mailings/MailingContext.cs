using Amazon.S3;
using Amazon.S3.Model;
using Beloyal.API.Chats;
using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Shops;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Mailings;

public class MailingContext(
  ApplicationDbContext dbContext,
  CustomerContext customerContext,
  ChatContext chatContext,
  AmazonS3Client s3Client
)
{
  public async Task<List<Mailing>> GetMailings(CompanyId companyId) =>
    await dbContext
      .Mailings.Where(v => v.CompanyId == companyId.Value)
      .Include(v => v.Units)
      .OrderByDescending(v => v.CreatedAt)
      .ToListAsync();

  public async Task UploadAttachments(Mailing mailing, List<ChatFileStream>? attachments = null)
  {
    if (attachments is { Count: > 0 })
    {
      var bucketName = mailing.Id.ToString();

      await s3Client.PutBucketAsync(
        new PutBucketRequest() { BucketName = bucketName, UseClientRegion = false }
      );

      dbContext.Update(mailing);
      mailing.AttachmentKeys = [];
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
        mailing.AttachmentKeys.Add(req.Key);
      }

      await dbContext.SaveChangesAsync();
    }
  }

  public async Task<Mailing> CreateMailing(
    Company company,
    string content,
    bool withShops,
    List<ShopId> shopIds,
    List<IFormFile>? attachments
  )
  {
    var mailing = new Mailing { Company = company, Content = content };

    List<CustomerId> customerIds = [];

    if (withShops && shopIds.Count > 0)
    {
      foreach (var shopId in shopIds)
      {
        var bonuses = (
          await dbContext.Bonuses.Where(v => v.ShopId == shopId.Value).ToListAsync()
        ).DistinctBy(v => v.CustomerId);

        customerIds = customerIds
          .Concat(bonuses.Select(vv => CustomerId.From(vv.CustomerId)))
          .ToList();
      }
    }

    customerIds = customerIds.DistinctBy(v => v.Value).ToList();

    var customers = withShops
      ? await customerContext.GetCustomers(CompanyId.From(company), customerIds)
      : await customerContext.GetCustomers(CompanyId.From(company));

    mailing.Units = customers
      .Select(v => new MailingUnit { Mailing = mailing, Customer = v })
      .ToList();

    dbContext.Add(mailing);
    await dbContext.SaveChangesAsync();

    await UploadAttachments(
      mailing,
      attachments
        ?.Select(v => new ChatFileStream(v.ContentType, v.FileName, v.OpenReadStream()))
        .ToList()
    );

    return mailing;
  }

  public async Task StartUnit(MailingUnit unit, List<ChatFileStream> chatFiles)
  {
    var customer = await customerContext.GetCustomer(CustomerId.From(unit.CustomerId));
    if (customer is null)
    {
      throw new Exception("invalid customer");
    }

    await dbContext.Entry(unit).Reference(v => v.Mailing).LoadAsync();

    dbContext.Update(unit);
    unit.Status = MailingUnitStatus.Done;

    var msg = await chatContext.CreateMessage(unit, customer, unit.Mailing.Content);
    await chatContext.SendMessage(msg, chatFiles, closeFileStream: false);
  }
}
