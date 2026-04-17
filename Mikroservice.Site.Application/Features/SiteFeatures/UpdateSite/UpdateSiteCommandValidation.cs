using FluentValidation;

namespace Mikroservice.Site.Application.Features.SiteFeatures.UpdateSite
{
    public class UpdateSiteCommandValidation : AbstractValidator<UpdateSiteCommand>
    {
        public UpdateSiteCommandValidation()
        {
            RuleFor(x => x.SiteAdi).NotEmpty();
            RuleFor(x => x.SiteUrl).NotEmpty();
            RuleFor(x => x.BirimId).GreaterThan(0);
            RuleFor(x => x.SiteAlanAdi).NotEmpty();
            RuleFor(x => x.SiteEPosta).NotEmpty();
        }
    }
}
