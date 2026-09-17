using FluentValidation;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.CreateGaleriResim
{
    public class CreateGaleriResimCommandValidation
        : AbstractValidator<CreateGaleriResimCommand>
    {
        public CreateGaleriResimCommandValidation()
        {
            RuleFor(x => x.SiteId).GreaterThan(0);
            RuleFor(x => x.DilId).GreaterThan(0);

            RuleFor(x => x.Baslik)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.ResimUrl)
                .NotEmpty()
                .WithMessage("Resim URL boş olamaz.");
            RuleFor(x => x.SeoUrl)
               .NotEmpty()
               .WithMessage("SEO URL boş olamaz.");
            RuleFor(x => x.Kategori)
                .MaximumLength(150);

            RuleFor(x => x.YayimTarihi)
                .LessThanOrEqualTo(DateTime.Now.AddYears(1));
        }
    }
}
