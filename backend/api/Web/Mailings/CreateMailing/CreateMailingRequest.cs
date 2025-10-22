namespace Beloyal.API.Web.Mailings.CreateMailing;

public record CreateMailingRequest(
  string Content,
  bool WithShops,
  List<Guid>? ShopIds,
  List<IFormFile>? Attachments
);
