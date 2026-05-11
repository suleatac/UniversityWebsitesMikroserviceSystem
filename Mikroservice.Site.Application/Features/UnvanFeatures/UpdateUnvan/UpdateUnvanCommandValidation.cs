using FluentValidation;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.UpdateUnvan
{
    public class UpdateUnvanCommandValidation : AbstractValidator<UpdateUnvanCommand>
    {
        public UpdateUnvanCommandValidation(IUnvanRepository repository)
        {
            RuleFor(x => x.TipId).GreaterThan(0);

            RuleFor(x => x.Ad)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.KisaAd)
                .NotEmpty()
                .MaximumLength(50);



        }
    }
}
