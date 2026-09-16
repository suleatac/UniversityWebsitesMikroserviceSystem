using FluentValidation;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.CreateSitePersonel
{
    public class CreateSitePersonelCommandValidation
        : AbstractValidator<CreateSitePersonelCommand>
    {
        public CreateSitePersonelCommandValidation()
        {
            RuleFor(x => x.SiteId).GreaterThan(0).WithMessage("SiteId 0'dan büyük olmalıdır.");
            RuleFor(x => x.PersonelId).GreaterThan(0).WithMessage("PersonelId 0'dan büyük olmalıdır.");
            RuleFor(x => x.UnvanId).GreaterThan(0).WithMessage("UnvanId 0'dan büyük olmalıdır.");
            RuleFor(x => x.PersonelTipId).GreaterThan(0).WithMessage("PersonelTipId 0'dan büyük olmalıdır.");
            RuleFor(x => x.PageTypeId).GreaterThan(0).WithMessage("PageTypeId 0'dan büyük olmalıdır.");
            RuleFor(x => x.SeoUrl).NotEmpty().MaximumLength(200);
            RuleFor(x => x.SeoTitle).MaximumLength(200);
            RuleFor(x => x.SeoDescription).MaximumLength(500);
        }
    }
}
