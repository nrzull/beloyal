using FluentValidation;

namespace Beloyal.API.Types;

public class Percent
{
  public decimal Value { get; set; }

  public Percent(decimal value)
  {
    Value = value;
    new Validator().ValidateAndThrow(this);
  }

  class Validator : AbstractValidator<Percent>
  {
    public Validator()
    {
      RuleFor(v => v.Value).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);
    }
  }
};
