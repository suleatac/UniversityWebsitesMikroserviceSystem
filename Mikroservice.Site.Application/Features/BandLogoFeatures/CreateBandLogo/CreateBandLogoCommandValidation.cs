using FluentValidation;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.CreateBandLogo
{
    public class CreateBandLogoCommandValidation : AbstractValidator<CreateBandLogoCommand>
    {
        public CreateBandLogoCommandValidation()
        {
            RuleFor(x => x.SiteId)
                   .GreaterThan(0).WithMessage("SiteId 0'dan büyük olmalıdır.");

            RuleFor(x => x.DilId)
                .GreaterThan(0)
                .WithMessage("DilId 0'dan büyük olmalıdır.");

            RuleFor(x => x.Ad)
                .NotEmpty().WithMessage("Ad alanı boş olamaz.")
                .MaximumLength(200).WithMessage("Ad en fazla 200 karakter olabilir.");

            RuleFor(x => x.ImgUrl)
                .NotEmpty().WithMessage("Görsel URL boş olamaz.")
                .MaximumLength(500).WithMessage("Görsel URL en fazla 500 karakter olabilir")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Geçerli bir URL giriniz.");

            RuleFor(x => x.Link)
                .MaximumLength(500).WithMessage("Link en fazla 500 karakter olabilir.")
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Geçerli bir link giriniz.");

        }
    }
}
