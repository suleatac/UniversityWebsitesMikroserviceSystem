using FluentValidation;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.UpdateSiteOzellikleri
{
    public class UpdateSiteOzellikleriCommandValidation : AbstractValidator<UpdateSiteOzellikleriCommand>
    {
        public UpdateSiteOzellikleriCommandValidation()
        {
            RuleFor(x => x.SiteId).GreaterThan(0);



        }
    }
}
