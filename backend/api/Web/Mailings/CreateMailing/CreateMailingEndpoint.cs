using Amazon.S3;
using Amazon.S3.Model;
using Beloyal.API.Chats;
using Beloyal.API.Companies;
using Beloyal.API.Mailings;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Mailings.CreateMailing;

public class CreateMailingEndpoint(
  ApplicationDbContext dbContext,
  MailingContext mailingContext,
  CompanyContext companyContext,
  AmazonS3Client s3Client
)
{
  public async Task<CreateMailingResponse> Handle(
    CompanyAccess companyAccess,
    CreateMailingRequest req
  )
  {
    var company = await companyContext.GetCompany(CompanyId.From(companyAccess));

    await using var tc = await dbContext.MaybeTransactionAsync();

    var mailing = await mailingContext.CreateMailing(
      company,
      req.Content,
      req.WithShops,
      req.ShopIds?.Select(v => ShopId.From(v)).ToList() ?? [],
      req.Attachments
    );

    await dbContext.SaveChangesAsync();

    List<ChatFileStream> files = [];
    if (mailing.AttachmentKeys is { Count: > 0 } attachmentKeys)
    {
      foreach (var attachmentKey in attachmentKeys)
      {
        var metadata = await s3Client.GetObjectMetadataAsync(
          new GetObjectMetadataRequest() { BucketName = mailing.Id.ToString(), Key = attachmentKey }
        );

        var file = await s3Client.GetObjectAsync(
          new GetObjectRequest() { BucketName = mailing.Id.ToString(), Key = attachmentKey }
        );

        files.Add(new ChatFileStream(metadata.ContentType, file.Key, file.ResponseStream));
      }
    }

    foreach (var group in mailing.Units.Chunk(10))
    {
      foreach (var unit in group)
      {
        await mailingContext.StartUnit(unit, files);
      }

      await tc.CommitAsync();
      await Task.Delay(TimeSpan.FromSeconds(1));
    }

    foreach (var file in files)
    {
      await file.Stream.DisposeAsync();
    }

    return new CreateMailingResponse.Ok(MailingDTO.From(mailing));
  }
}
