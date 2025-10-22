using Amazon.S3;
using Beloyal.API.Chats;
using Beloyal.API.Companies;

namespace Beloyal.API.Web.Chats.GetChats;

public class GetChatsEndpoint(ChatContext chatContext, AmazonS3Client s3Client)
{
  public async Task<GetChatsResponse> Handle(CompanyAccess companyAccess)
  {
    var chats = await chatContext.GetChats(CompanyId.From(companyAccess));

    return new GetChatsResponse.Ok(chats.Select((v) => ChatDTO.From(v, s3Client)).ToList());
  }
}
