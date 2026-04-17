using FluentValidation;

namespace Mikroservice.Site.Application.Features.SiteFeatures.CreateSite
{
    public class CreateSiteCommandValidation : AbstractValidator<CreateSiteCommand>
    {
        public CreateSiteCommandValidation()
        {
            RuleFor(x => x.SiteAdi).NotEmpty();
            RuleFor(x => x.SiteUrl).NotEmpty();
            RuleFor(x => x.BirimId).GreaterThan(0);
            RuleFor(x => x.SiteAlanAdi).NotEmpty();
            RuleFor(x => x.SiteEPosta).NotEmpty();
        }
    }
}
