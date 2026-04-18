using FluentValidation;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.CreateSitePersonel
{
    public class CreateSitePersonelCommandValidation
        : AbstractValidator<CreateSitePersonelCommand>
    {
        public CreateSitePersonelCommandValidation()
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
