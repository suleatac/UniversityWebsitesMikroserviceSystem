using FluentValidation;
using Microservice.Site.Application.Features.PageTypeFeatures.UpdatePageType;
using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.UpdatePageType
{
    public class UpdatePageTypeCommandValidation : AbstractValidator<UpdatePageTypeCommand>
    {
        public UpdatePageTypeCommandValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.SiteId).GreaterThan(0);
            RuleFor(x => x.TemplateId).GreaterThan(0);
            RuleFor(x => x.PageTypeId).IsInEnum();
            RuleFor(x => x.DilId).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ViewName).MaximumLength(200).When(x => x.ViewName is not null);
        }
    }
}