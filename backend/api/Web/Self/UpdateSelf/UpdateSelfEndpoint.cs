using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.UpdateSelf;

public class UpdateSelfEndpoint(UserContext userContext)
{
  public async Task<UpdateSelfResponse> Handle(User user, UpdateSelfRequest req)
  {
    await userContext.UpdateUserInfo(
      user,
      firstName: req.FirstName,
      lastName: req.LastName,
      middleName: req.MiddleName,
      countryLang: req.CountryLang
    );

    return new UpdateSelfResponse.Ok();
  }
}
