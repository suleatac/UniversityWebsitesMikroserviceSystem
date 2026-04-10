using FluentValidation;

namespace Microservice.Yonetici.Application.Features.YoneticiDuyuruFeatures.CreateYoneticiDuyuru
{
    public class CreateYoneticiDuyuruCommandValidation : AbstractValidator<CreateYoneticiDuyuruCommand>
    {

        public CreateYoneticiDuyuruCommandValidation()
        {
            RuleFor(x => x.Baslik).NotEmpty().WithMessage("Başlık boş olamaz.").MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");
            RuleFor(x => x.Icerik).NotEmpty().WithMessage("İçerik boş olamaz.");
            RuleFor(x => x.EklenmeTarihi).NotEmpty().WithMessage("Eklenme tarihi boş olamaz.");
            RuleFor(x => x.Aktif).NotNull().WithMessage("Aktif durumu boş olamaz.");
        }


    }
}
