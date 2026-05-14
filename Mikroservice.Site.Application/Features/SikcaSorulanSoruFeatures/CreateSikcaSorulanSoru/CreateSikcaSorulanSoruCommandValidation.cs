using FluentValidation;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.CreateSikcaSorulanSoru
{
    public class CreateSikcaSorulanSoruCommandValidation
       : AbstractValidator<CreateSikcaSorulanSoruCommand>
    {
        public CreateSikcaSorulanSoruCommandValidation()
        {
            // 🔹 SiteId
            RuleFor(x => x.SiteId)
                .GreaterThan(0).WithMessage("Geçerli bir SiteId girilmelidir.");

            // 🔹 DilId
            RuleFor(x => x.DilId)
                .GreaterThan(0).WithMessage("Geçerli bir DilId girilmelidir.");


            // 🔹 Soru
            RuleFor(x => x.Soru)
                .NotEmpty().WithMessage("Soru boş olamaz.")
                .MaximumLength(500).WithMessage("Soru en fazla 500 karakter olabilir.");

            // 🔹 Cevap
            RuleFor(x => x.Cevap)
                .NotEmpty().WithMessage("Cevap boş olamaz.");

            // 🔹 Sıra
            RuleFor(x => x.Sira)
                .GreaterThanOrEqualTo(0).WithMessage("Sıra 0 veya daha büyük olmalıdır.");

            // 🔹 SeoUrl (opsiyonel ama düzgün olsun)
            RuleFor(x => x.SeoUrl)
                .MaximumLength(300)
                .When(x => !string.IsNullOrEmpty(x.SeoUrl))
                .WithMessage("SeoUrl en fazla 300 karakter olabilir.");


        }
    }
}
