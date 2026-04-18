using FluentValidation;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.CreateYoneticiSite
{
    public class CreateYoneticiSiteCommandValidation
    : AbstractValidator<CreateYoneticiSiteCommand>
    {
        public CreateYoneticiSiteCommandValidation()
        {
            RuleFor(x => x.KeycloakUserId)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.SiteId)
                .GreaterThan(0);

            RuleFor(x => x.YoneticiTipiId)
                .GreaterThan(0);
        }
    }
}
