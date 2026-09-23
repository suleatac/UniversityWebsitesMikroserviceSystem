using FluentValidation;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.CreateEtkinlik
{
    public class CreateEtkinlikCommandValidation : AbstractValidator<CreateEtkinlikCommand>
    {
        public CreateEtkinlikCommandValidation()
        {
            RuleFor(x => x.SiteId)
               .GreaterThan(0).WithMessage("SiteId 0'dan büyük olmalıdır.");

            RuleFor(x => x.DilId)
                .GreaterThan(0).WithMessage("DilId 0'dan büyük olmalıdır.");

            RuleFor(x => x.HedefId)
                .GreaterThan(0)
                .WithMessage("HedefId 0'dan büyük olmalıdır.");

            RuleFor(x => x.Baslik)
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(x => x.KisaAciklama)
                .MaximumLength(500).WithMessage("Kısa açıklama en fazla 500 karakter olabilir.");



            RuleFor(x => x.ResimUrl)
                .NotEmpty().WithMessage("Resim URL boş olamaz.")
                .MaximumLength(500).WithMessage("Resim URL en fazla 500 karakter olabilir.");

            RuleFor(x => x.Link)
                .MaximumLength(500).WithMessage("Link en fazla 500 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Link))
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Geçerli bir link giriniz.");

            RuleFor(x => x.BaslamaTarihi)
                .LessThan(x => x.BitisTarihi!.Value)
                .When(x => x.BaslamaTarihi.HasValue && x.BitisTarihi.HasValue)
                .WithMessage("Başlama tarihi, bitiş tarihinden önce olmalıdır.");

            RuleFor(x => x.YayimTarihi).NotEmpty().WithMessage("Yayım tarihi boş olamaz.")
                .LessThan(x => x.BitisTarihi!.Value)
                .When(x => x.BitisTarihi.HasValue)
                .WithMessage("Yayım tarihi, bitiş tarihinden önce olmalıdır.");

            RuleFor(x => x.SeoUrl).NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.SeoTitle)
                .MaximumLength(200);

            RuleFor(x => x.SeoDescription)
                .MaximumLength(500);

        }
    }
}
