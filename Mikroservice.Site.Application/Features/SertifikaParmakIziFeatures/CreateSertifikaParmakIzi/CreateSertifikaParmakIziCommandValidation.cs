using FluentValidation;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.CreateSertifikaParmakIzi
{
    public class CreateSertifikaParmakIziCommandValidation : AbstractValidator<CreateSertifikaParmakIziCommand>
    {
        public CreateSertifikaParmakIziCommandValidation()
        {
            RuleFor(x => x.SertifikaParmakIziNumarasi).NotEmpty();
        }
    }
}
