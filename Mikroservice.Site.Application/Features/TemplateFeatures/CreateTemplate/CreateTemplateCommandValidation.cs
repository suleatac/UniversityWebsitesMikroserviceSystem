using FluentValidation;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.CreateTemplate
{
    public class CreateTemplateCommandValidation : AbstractValidator<CreateTemplateCommand>
    {
        public CreateTemplateCommandValidation(ITemplateRepository templateRepository)
        {
            RuleFor(x => x.TemplateAdi).NotEmpty().MaximumLength(200);
            RuleFor(x => x.TemplateTuru).NotEmpty();

        }
    }
}
