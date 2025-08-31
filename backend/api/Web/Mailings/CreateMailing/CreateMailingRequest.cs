namespace Beloyal.API.Web.Mailings.CreateMailing;

public record CreateMailingRequest(string Text, bool WithShops, List<Guid> ShopIds);
