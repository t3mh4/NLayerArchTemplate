using FluentValidation;

namespace NLayerArchTemplate.Business.Validators.User;

public class UserIdValidator : AbstractValidator<int>
{
    public UserIdValidator()
    {
        RuleFor(r => r).GreaterThanOrEqualTo(0).WithName("Kullanıcı Id");
    }
}