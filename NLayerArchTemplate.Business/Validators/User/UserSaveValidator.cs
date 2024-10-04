using FluentValidation;
using NLayerArchTemplate.Dtos.User;

namespace NLayerArchTemplate.Business.Validators.User;

public class UserSaveValidator : AbstractValidator<UserSaveDto>
{
    public UserSaveValidator()
    {
        RuleFor(r => r.Id).GreaterThanOrEqualTo(0).WithName("Kullanıcı Id");

        RuleFor(x => x.Username).NotEmpty()
                                .CustomRule(50)
                                .WithName("Kullanıcı Adı");
        When(w => w.Id == 0, () =>
        {
            RuleFor(x => x.Password).NotEmpty()
                                    .CustomRule(250)
                                    .WithName("Şifre");
        }).Otherwise(() =>
        {
            RuleFor(x => x.Password).Must(u => !u.Any(x => char.IsWhiteSpace(x)));
        });
        RuleFor(x => x.Name).NotEmpty()
                            .CustomRule(75)
                            .WithName("Ad");
        RuleFor(x => x.Surname).NotEmpty()
                               .CustomRule(75)
                               .WithName("Soyad");
        RuleFor(x => x.Email).EmailAddress()
                             .CustomRule(250)
                             .WithName("Email");
    }
}
