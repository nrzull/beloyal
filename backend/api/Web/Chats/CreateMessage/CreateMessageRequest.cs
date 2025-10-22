namespace Beloyal.API.Web.Chats.CreateMessage;

public record CreateMessageRequest(string Content, List<IFormFile>? Attachments);
