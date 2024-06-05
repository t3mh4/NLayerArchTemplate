using FluentValidation;

namespace NLayerArchTemplate.Business.Validators.User;

public class UserDeleteValidator : AbstractValidator<int>
{
    public UserDeleteValidator()
    {
        RuleFor(r => r).GreaterThan(0).WithName("Kullanıcı Id");
    }
}