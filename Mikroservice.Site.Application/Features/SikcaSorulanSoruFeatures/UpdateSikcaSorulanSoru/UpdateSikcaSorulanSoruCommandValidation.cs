using FluentValidation;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.UpdateSikcaSorulanSoru
{
    public class UpdateSikcaSorulanSoruCommandValidation : AbstractValidator<UpdateSikcaSorulanSoruCommand>
    {
        public UpdateSikcaSorulanSoruCommandValidation(ISikcaSorulanSoruKategoriRepository kategoriRepository)
        {
            // 🔹 SiteId
            RuleFor(x => x.SiteId)
                .GreaterThan(0).WithMessage("Geçerli bir SiteId girilmelidir.");

            // 🔹 DilId
            RuleFor(x => x.DilId)
                .GreaterThan(0).WithMessage("Geçerli bir DilId girilmelidir.");

            // 🔹 KategoriId
            RuleFor(x => x.KategoriId)
                .GreaterThan(0).WithMessage("Geçerli bir KategoriId girilmelidir.");

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
            // 🔥 Kategori var mı kontrolü
            RuleFor(x => x).MustAsync(async (request, cancellationToken) =>
            {
                return await kategoriRepository.AnyByIdAsync(request.KategoriId, cancellationToken);
            }).WithMessage("Kategori aynı site ve dile ait olmalıdır.");

        }
    }
}
