using FluentValidation;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.CreatePageType
{
    public class CreatePageTypeCommandValidation : AbstractValidator<CreatePageTypeCommand>
    {
        public CreatePageTypeCommandValidation()
        {
            RuleFor(x => x.TemplateId).GreaterThan(0);
            RuleFor(x => x.PageTypeKind).IsInEnum();
            RuleFor(x => x.DilId).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ViewName).MaximumLength(200).When(x => x.ViewName is not null);
        }
    }
}