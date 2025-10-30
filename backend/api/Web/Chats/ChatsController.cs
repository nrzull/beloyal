using Beloyal.API.Chats;
using Beloyal.API.Companies;
using Beloyal.API.Web.Chats.CreateMessage;
using Beloyal.API.Web.Chats.GetChat;
using Beloyal.API.Web.Chats.GetChatMessages;
using Beloyal.API.Web.Chats.GetChats;
using Beloyal.API.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Chats;

[ApiController]
[Route("/api/company/chats")]
[WithAuth]
[WithCompanyAccess]
public class ChatsController : ControllerBase
{
  [HttpGet]
  [WithCompanyPermissions(nameof(CompanyPermissions.chats_read))]
  public async Task<Results<Ok<GetChatsResponse.Ok>, BadRequest>> GetChats(
    [FromServices] GetChatsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetChatsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}")]
  [WithCompanyPermissions(nameof(CompanyPermissions.chats_read))]
  public async Task<Results<Ok<GetChatResponse.Ok>, BadRequest, NotFound>> GetChat(
    [FromServices] GetChatEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid chatId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ChatId.From(chatId)) switch
    {
      GetChatResponse.Ok ok => TypedResults.Ok(ok),
      GetChatResponse.NotFound => TypedResults.NotFound(),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}/messages")]
  [WithCompanyPermissions(nameof(CompanyPermissions.chats_read))]
  public async Task<Results<Ok<GetChatMessagesResponse.Ok>, BadRequest>> GetChatMessages(
    [FromServices] GetChatMessagesEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid chatId,
    [FromQuery(Name = "withSystem")] bool withSystem
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ChatId.From(chatId), withSystem) switch
    {
      GetChatMessagesResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost("{id}/messages")]
  [WithCompanyPermissions(nameof(CompanyPermissions.chats_write))]
  public async Task<Results<Ok<CreateMessageResponse.Ok>, BadRequest>> CreateMessage(
    [FromServices] CreateMessageEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid chatId,
    [FromForm] CreateMessageRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), ChatId.From(chatId), req) switch
    {
      CreateMessageResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
