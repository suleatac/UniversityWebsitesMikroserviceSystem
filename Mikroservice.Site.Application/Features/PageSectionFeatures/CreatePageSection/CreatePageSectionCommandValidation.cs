using FluentValidation;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.CreatePageSection
{
    public class CreatePageSectionCommandValidation : AbstractValidator<CreatePageSectionCommand>
    {
        public CreatePageSectionCommandValidation()
        {
            RuleFor(x => x.Baslik)
                .NotEmpty().WithMessage("Bölüm başlığı boş olamaz.")
                .MaximumLength(200).WithMessage("Bölüm başlığı en fazla 200 karakter olabilir.");

            RuleFor(x => x.SiteId)
                .GreaterThan(0).WithMessage("Geçerli bir SiteId girilmelidir.");

            RuleFor(x => x.DilId)
                .GreaterThan(0).WithMessage("Geçerli bir DilId girilmelidir.");

            RuleFor(x => x.BackgroundColor)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.BackgroundColor))
                .WithMessage("Arka plan rengi en fazla 50 karakter olabilir.");

            RuleFor(x => x.BackgroundImageUrl)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.BackgroundImageUrl))
                .WithMessage("Arka plan görseli en fazla 500 karakter olabilir.");
        }
    }
}
