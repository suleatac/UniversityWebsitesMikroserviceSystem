using FluentValidation;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.CreateSiteOzellikleri
{
    public class CreateSiteOzellikleriCommandValidation
        : AbstractValidator<CreateSiteOzellikleriCommand>
    {
        public CreateSiteOzellikleriCommandValidation()
        {
            RuleFor(x => x.SiteId).GreaterThan(0);

 


        }
    }
}
