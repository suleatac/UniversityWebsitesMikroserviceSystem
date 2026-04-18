using FluentValidation;

namespace Mikroservice.Site.Application.Features.SiteFeatures.UpdateSite
{
    public class UpdateSiteCommandValidation : AbstractValidator<UpdateSiteCommand>
    {
        public UpdateSiteCommandValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x.SiteAdi).NotEmpty();

            RuleFor(x => x.SiteUrl)
                .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
                .WithMessage("Geçerli URL giriniz.");
        }
    }
}
