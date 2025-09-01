using Beloyal.API.Chats;
using Beloyal.API.Companies;
using Beloyal.API.Web.Chats.GetChatMessages;
using Beloyal.API.Web.Chats.GetChats;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Chats;

[ApiController]
[Route("/api/company/chats")]
[WithAuth]
[WithCompanyAccess]
public class ChatsController : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetChats(
    [FromServices] GetChatsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetChatsResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpGet("{id}/messages")]
  public async Task<IActionResult> GetChatMessages(
    [FromServices] GetChatMessagesEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid chatId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ChatId.From(chatId)) switch
    {
      GetChatMessagesResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
