using FluentValidation;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.UpdateSertifikaParmakIzi
{
    public class UpdateSertifikaParmakIziCommandValidation : AbstractValidator<UpdateSertifikaParmakIziCommand>
    {
        public UpdateSertifikaParmakIziCommandValidation()
        {
            RuleFor(x => x.SertifikaParmakIziNumarasi).NotEmpty().WithMessage("SertifikaParmakIziNumarasi alanı boş olamaz.");
            RuleFor(x => x.SertifikaYili).NotEmpty().WithMessage("SertifikaYili alanı boş olamaz.");
            RuleFor(x => x.Aktif).NotNull().WithMessage("Aktif alanı boş olamaz.");
        }
    }
}
