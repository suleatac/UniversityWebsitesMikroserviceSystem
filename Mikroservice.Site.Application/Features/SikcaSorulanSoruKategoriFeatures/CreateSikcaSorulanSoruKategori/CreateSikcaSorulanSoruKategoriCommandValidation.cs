using FluentValidation;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.CreateSikcaSorulanSoruKategori
{
    public class CreateSikcaSorulanSoruKategoriCommandValidation : AbstractValidator<CreateSikcaSorulanSoruKategoriCommand>
    {
        public CreateSikcaSorulanSoruKategoriCommandValidation()
        {
            // 🔹 Ad
            RuleFor(x => x.Ad)
                .NotEmpty().WithMessage("Sıkça sorulan soruların kategori adı boş olamaz.")
                .MaximumLength(200).WithMessage("Sıkça sorulan soruların kategori adı en fazla 200 karakter olabilir.");
            // 🔹 Sira
            RuleFor(x => x.Sira)
                .GreaterThanOrEqualTo(0).WithMessage("Sıra 0 veya daha büyük olmalıdır.");
        }
    }
}
