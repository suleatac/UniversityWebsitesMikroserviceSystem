using FluentValidation;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.UpdatePersonelTip
{
    public class UpdatePersonelTipCommandValidation : AbstractValidator<UpdatePersonelTipCommand>
    {
        public UpdatePersonelTipCommandValidation()
        {
            RuleFor(x => x.Ad).NotEmpty();
        }
    }
}
