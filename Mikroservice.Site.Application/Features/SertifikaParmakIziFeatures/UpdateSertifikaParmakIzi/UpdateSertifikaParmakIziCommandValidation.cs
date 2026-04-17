using FluentValidation;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.UpdateSertifikaParmakIzi
{
    public class UpdateSertifikaParmakIziCommandValidation : AbstractValidator<UpdateSertifikaParmakIziCommand>
    {
        public UpdateSertifikaParmakIziCommandValidation()
        {
            RuleFor(x => x.SertifikaParmakIziNumarasi).NotEmpty();
        }
    }
}
