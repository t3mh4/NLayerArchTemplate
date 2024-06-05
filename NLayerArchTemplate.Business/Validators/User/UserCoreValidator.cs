using FluentValidation;

namespace NLayerArchTemplate.Business.Validators.User;

public class UserCoreValidator : AbstractValidator<int>
{
    public UserCoreValidator()
    {
        RuleFor(r => r).GreaterThanOrEqualTo(0).WithName("Kullanıcı Id");
    }
}