using FluentValidation;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Dtos.User;

namespace NLayerArchTemplate.Business.Validators.User;

public class UserDeleteValidator : AbstractValidator<int>
{
    public UserDeleteValidator()
    {
        RuleFor(r => r).GreaterThanOrEqualTo(0)
                       .WithName("Kullanıcı Id")
                       .NotEqual(UserEnum.Admin.ToInt32())
                       .WithMessage("Admin kullanıcısı silinemez..!!");
    }
}
