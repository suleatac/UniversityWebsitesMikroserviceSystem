using FluentValidation;

namespace Mikroservice.Site.Application.Features.BirimFeatures.CreateBirim
{
    public class CreateBirimCommandValidation : AbstractValidator<CreateBirimCommand>
    {
        public CreateBirimCommandValidation() {

            RuleFor(x => x.Ad)
                   .NotEmpty().WithMessage("Birim adı boş olamaz.")
                   .MaximumLength(200).WithMessage("Birim adı en fazla 200 karakter olabilir.");

            RuleFor(x => x.ParentId)
                .GreaterThan(0)
                .When(x => x.ParentId.HasValue)
                .WithMessage("ParentId 0'dan büyük olmalıdır.");

            RuleFor(x => x.Sira)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Sıra 0 veya daha büyük olmalıdır.");


        }
    }
}
