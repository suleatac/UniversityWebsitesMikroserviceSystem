using FluentValidation;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.UpdateSiteOzellikleri
{
    public class UpdateSiteOzellikleriCommandValidation : AbstractValidator<UpdateSiteOzellikleriCommand>
    {
        public UpdateSiteOzellikleriCommandValidation()
        {
            RuleFor(x => x.SiteId).GreaterThan(0);

            RuleFor(x => x.SiteAdress).NotEmpty();
            RuleFor(x => x.SiteTelNo).NotEmpty();

            RuleFor(x => x.SiteFacebookAdress)
                .Must(x => string.IsNullOrEmpty(x) || Uri.IsWellFormedUriString(x, UriKind.Absolute))
                .WithMessage("Geçerli bir URL giriniz.");
        }
    }
}
