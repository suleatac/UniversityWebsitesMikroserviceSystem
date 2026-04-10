using FluentValidation;

namespace Microservice.Yonetici.Application.Features.YoneticiTipiFeatures.CreateYoneticiTipi
{
    public class CreateYoneticiTipiCommandValidation : AbstractValidator<CreateYoneticiTipiCommand>
    {
        public CreateYoneticiTipiCommandValidation()
        {
            RuleFor(x => x.TipAdi).NotEmpty().WithMessage("Tip adı boş olamaz.");
            RuleFor(x => x.Value).GreaterThan(0).WithMessage("Value sıfırdan büyük olmalıdır.");
        }
    }
}
