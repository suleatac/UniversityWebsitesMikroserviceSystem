using FluentValidation;

namespace Mikroservice.Site.Application.Features.PopupFeatures.CreatePopup
{
    public class CreatePopupCommandValidation : AbstractValidator<CreatePopupCommand>
    {
        public CreatePopupCommandValidation()
        {
            RuleFor(x => x.SiteId)
                  .GreaterThan(0).WithMessage("SiteId 0'dan büyük olmalıdır.");

            RuleFor(x => x.DilId)
                .GreaterThan(0).WithMessage("DilId 0'dan büyük olmalıdır.");

            RuleFor(x => x.HedefId)
                .GreaterThan(0)
                .When(x => x.HedefId.HasValue)
                .WithMessage("HedefId 0'dan büyük olmalıdır.");

            RuleFor(x => x.Baslik)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(x => x.KisaAciklama)
                .NotEmpty().WithMessage("Kısa açıklama boş olamaz.")
                .MaximumLength(500).WithMessage("Kısa açıklama en fazla 500 karakter olabilir.");

            RuleFor(x => x.IcerikMetni)
                .NotEmpty().WithMessage("İçerik metni boş olamaz.");

            RuleFor(x => x.ResimUrl)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.ResimUrl))
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Geçerli bir resim URL giriniz.");

            RuleFor(x => x.Link)
                .MaximumLength(500)
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

            RuleFor(x => x.SeoUrl)
                .MaximumLength(200);

            RuleFor(x => x.SeoTitle)
                .MaximumLength(200);

            RuleFor(x => x.SeoDescription)
                .MaximumLength(500);
            RuleFor(x => x)
                .Must(x => x.TamEkranMi || x.GosterimSuresiSaniye > 0)
                .WithMessage("Tam ekran değilse, gösterim süresi saniye cinsinden 0'dan büyük olmalıdır.");
        
        RuleFor(x => x)
                .Must(x => !x.CookieIleTekrarGosterme || (x.GosterimSuresiSaniye > 0))
                .WithMessage("Cookie ile tekrar gösterme seçeneği aktifse, gösterim süresi saniye cinsinden 0'dan büyük olmalıdır.");
        }
    }
}
