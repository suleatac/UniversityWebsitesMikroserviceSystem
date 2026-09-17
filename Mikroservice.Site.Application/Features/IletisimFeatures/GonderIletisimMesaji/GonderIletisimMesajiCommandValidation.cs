using FluentValidation;

namespace Mikroservice.Site.Application.Features.IletisimFeatures.GonderIletisimMesaji
{
    public class GonderIletisimMesajiCommandValidation
        : AbstractValidator<GonderIletisimMesajiCommand>
    {
        public GonderIletisimMesajiCommandValidation()
        {
            RuleFor(x => x.SiteId).GreaterThan(0);

            RuleFor(x => x.AdSoyad)
                .NotEmpty().WithMessage("Ad Soyad boş olamaz.")
                .MaximumLength(150);

            RuleFor(x => x.Eposta)
                .NotEmpty().WithMessage("E-posta boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(200);

            RuleFor(x => x.Konu)
                .NotEmpty().WithMessage("Konu boş olamaz.")
                .MaximumLength(300);

            RuleFor(x => x.Mesaj)
                .NotEmpty().WithMessage("Mesaj boş olamaz.")
                .MaximumLength(5000);
        }
    }
}
