using FluentValidation;
using NLayerArchTemplate.Dtos.User;

namespace NLayerArchTemplate.Business.Validators.User;

public class UserSaveValidator : AbstractValidator<UserSaveDto>
{
    public UserSaveValidator()
    {
        RuleFor(x => x.Username).NotEmpty().NotNull().WithName("Kullanıcı Adı");
        When(w => w.Id == 0, () =>
        {
            RuleFor(x => x.Password).NotEmpty().NotNull().WithName("Şifre");
        }).Otherwise(() =>
        {
            RuleFor(x => x.Password).Must(u => !u.Any(x => char.IsWhiteSpace(x)));
        });
        RuleFor(x => x.Name).NotEmpty().NotNull().WithName("Ad");
        RuleFor(x => x.Surname).NotEmpty().NotNull().WithName("Soyad");
    }
}
