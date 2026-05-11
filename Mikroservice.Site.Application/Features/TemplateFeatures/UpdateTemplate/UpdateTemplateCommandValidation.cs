using FluentValidation;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.UpdateTemplate
{
    public class UpdateTemplateCommandValidation : AbstractValidator<UpdateTemplateCommand>
    {
        public UpdateTemplateCommandValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.TemplateAdi).NotEmpty().MaximumLength(200);
            RuleFor(x => x.TemplateTuru).NotEmpty();
        }
    }
}
