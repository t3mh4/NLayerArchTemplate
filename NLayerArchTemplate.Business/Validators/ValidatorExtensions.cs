using FluentValidation;

namespace NLayerArchTemplate.Business.Validators;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> NoSpecialCharacters<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(@"^[^'""=]*$")
            .WithMessage("'{PropertyName}' için ', \" ve = karakterlerini kullanmayınız.");
    }
    public static IRuleBuilderOptions<T, string> CustomRule<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NoSpecialCharacters();
    }

    public static IRuleBuilderOptions<T, string> CustomRule<T>(this IRuleBuilder<T, string> ruleBuilder, int maxlength)
    {
        return ruleBuilder
            .NoSpecialCharacters()
            .MaximumLength(maxlength);
    }
}
