using FluentValidation;

namespace Beloyal.API.Types;

public class Email
{
  public string Value { get; set; }

  public Email(string value)
  {
    Value = value;
    new Validator().ValidateAndThrow(this);
  }

  class Validator : AbstractValidator<Email>
  {
    public Validator()
    {
      RuleFor(v => v.Value).NotEmpty().EmailAddress();
    }
  }
};
