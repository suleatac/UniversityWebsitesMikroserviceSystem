using FluentValidation;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.CreatePersonelTip
{
    public class CreatePersonelTipCommandValidation : AbstractValidator<CreatePersonelTipCommand>
    {
        public CreatePersonelTipCommandValidation()
        {
            RuleFor(x => x.Ad).NotEmpty();
        }
    }
}
