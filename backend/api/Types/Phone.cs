using FluentValidation;

namespace Beloyal.API.Types;

public class Phone
{
  public string Value { get; set; }

  public Phone(string value)
  {
    Value = value;
    new Validator().ValidateAndThrow(this);
  }

  class Validator : AbstractValidator<Phone>
  {
    public Validator()
    {
      RuleFor(v => v.Value)
        .Matches(@"^\+\d{11,15}$", System.Text.RegularExpressions.RegexOptions.Multiline);
    }
  }
};
