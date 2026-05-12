using FluentValidation;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.UpdateYoneticiSite
{
    public class UpdateYoneticiSiteCommandValidation
      : AbstractValidator<UpdateYoneticiSiteCommand>
    {
        public UpdateYoneticiSiteCommandValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x.YoneticiTipiId)
                .GreaterThan(0);

            RuleFor(x => x.KeycloakUserId)
           .NotEmpty();
        }
    }
}
