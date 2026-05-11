using FluentValidation;
using Microservice.Site.Application.Features.YonetimDuyuruFeatures.CreateYonetimDuyuru;

namespace Microservice.Site.Application.Features.YoneticiDuyuruFeatures.CreateYoneticiDuyuru
{
    public class CreateYonetimDuyuruCommandValidation : AbstractValidator<CreateYonetimDuyuruCommand>
    {

        public CreateYonetimDuyuruCommandValidation()
        {
            RuleFor(x => x.Baslik).NotEmpty().WithMessage("Başlık boş olamaz.").MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");
            RuleFor(x => x.Icerik).NotEmpty().WithMessage("İçerik boş olamaz.");
     
        }


    }
}
