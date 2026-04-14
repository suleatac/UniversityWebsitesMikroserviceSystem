using FluentValidation;

namespace Microservice.Site.Application.Features.YoneticiTipiFeatures.UpdateYoneticiTipi
{
    public class UpdateYoneticiTipiCommandValidation : AbstractValidator<UpdateYoneticiTipiCommand>
    {
        public UpdateYoneticiTipiCommandValidation()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id alanı boş olamaz.");
            RuleFor(x => x.TipAdi).NotEmpty().WithMessage("Tip adı boş olamaz.");
            RuleFor(x => x.Value).GreaterThan(0).WithMessage("Value sıfırdan büyük olmalıdır.");
        }
    }
}
