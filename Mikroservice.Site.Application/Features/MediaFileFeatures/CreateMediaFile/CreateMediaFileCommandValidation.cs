using FluentValidation;

namespace Mikroservice.Site.Application.Features.MediaFileFeatures.CreateMediaFile
{
    public class CreateMediaFileCommandValidation : AbstractValidator<CreateMediaFileCommand>
    {
        public CreateMediaFileCommandValidation()
        {
            RuleFor(x => x.SiteId)
                  .GreaterThan(0).WithMessage("SiteId 0'dan büyük olmalıdır.");

            RuleFor(x => x.DilId)
                .GreaterThan(0).WithMessage("DilId 0'dan büyük olmalıdır.");

            RuleFor(x => x.Url)
           .MaximumLength(500)
           .When(x => !string.IsNullOrEmpty(x.Url))
           .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
           .WithMessage("Geçerli bir URL giriniz.");

            RuleFor(x => x.Path)
           .MaximumLength(500)
           .When(x => !string.IsNullOrEmpty(x.Path))
           .WithMessage("Geçerli bir path giriniz.");
        }
    }
}
