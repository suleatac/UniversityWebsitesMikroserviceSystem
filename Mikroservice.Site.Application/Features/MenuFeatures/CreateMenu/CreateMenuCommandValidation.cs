using FluentValidation;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.MenuFeatures.CreateMenu
{
    public class CreateMenuCommandValidation : AbstractValidator<CreateMenuCommand>
    {
        public CreateMenuCommandValidation(IMenuRepository menuRepository)
        {
            // 🔹 Ad
            RuleFor(x => x.Baslik)
                .NotEmpty().WithMessage("Menü adı boş olamaz.")
                .MaximumLength(200).WithMessage("Menü adı en fazla 200 karakter olabilir.");

            // 🔹 Link
            RuleFor(x => x.Link).MaximumLength(500).WithMessage("Link en fazla 500 karakter olabilir.");

            // 🔹 SiteId
            RuleFor(x => x.SiteId)
                .GreaterThan(0).WithMessage("Geçerli bir SiteId girilmelidir.");

            // 🔹 DilId
            RuleFor(x => x.DilId)
                .GreaterThan(0).WithMessage("Geçerli bir DilId girilmelidir.");

            // 🔹 HedefId
            RuleFor(x => x.HedefId)
                .GreaterThan(0).WithMessage("Geçerli bir HedefId girilmelidir.");

            // 🔹 Sira
            RuleFor(x => x.Sira)
                .GreaterThanOrEqualTo(0).WithMessage("Sıra 0 veya daha büyük olmalıdır.");

            // 🔹 SeoUrl
            RuleFor(x => x.SeoUrl).NotEmpty()
                .MaximumLength(200);

            // 🔹 IcerikMetni (opsiyonel)
            RuleFor(x => x.IcerikMetni)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrEmpty(x.IcerikMetni))
                .WithMessage("İçerik en fazla 2000 karakter olabilir.");
        }
    }
}
