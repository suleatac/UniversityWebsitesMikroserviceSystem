using FluentValidation;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.UpdateSitePersonel
{
    public class UpdateSitePersonelCommandValidation
        : AbstractValidator<UpdateSitePersonelCommand>
    {
        public UpdateSitePersonelCommandValidation()
        {
            RuleFor(x => x.SiteId).GreaterThan(0);
            RuleFor(x => x.PersonelId).GreaterThan(0);
            RuleFor(x => x.UnvanId).GreaterThan(0);
            RuleFor(x => x.PersonelTipId).GreaterThan(0);

            RuleFor(x => x.ResimUrl)
                .NotEmpty();

            RuleFor(x => x.BlogAdress)
                .Must(x => string.IsNullOrEmpty(x) || Uri.IsWellFormedUriString(x, UriKind.Absolute));

            RuleFor(x => x.TwitterAdress)
                .Must(x => string.IsNullOrEmpty(x) || Uri.IsWellFormedUriString(x, UriKind.Absolute));

            RuleFor(x => x.FacebookAdress)
                .Must(x => string.IsNullOrEmpty(x) || Uri.IsWellFormedUriString(x, UriKind.Absolute));
        }
    }
}
