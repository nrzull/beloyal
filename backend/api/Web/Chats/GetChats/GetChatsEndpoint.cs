using Beloyal.API.Chats;
using Beloyal.API.Companies;

namespace Beloyal.API.Web.Chats.GetChats;

public class GetChatsEndpoint(ChatContext chatContext)
{
  public async Task<GetChatsResponse> Handle(CompanyAccess companyAccess)
  {
    var chats = await chatContext.GetChats(CompanyId.From(companyAccess));

    return new GetChatsResponse.Ok(chats.Select(ChatDTO.From).ToList());
  }
}
