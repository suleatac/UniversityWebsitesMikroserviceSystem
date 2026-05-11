using FluentValidation;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.UpdateYonetimDuyuru  
{
    public class UpdateYonetimDuyuruCommandValidation : AbstractValidator<UpdateYonetimDuyuruCommand>
    {

        public UpdateYonetimDuyuruCommandValidation()
        {
            RuleFor(x => x.Baslik).NotEmpty().WithMessage("Başlık boş olamaz.").MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");
            RuleFor(x => x.Icerik).NotEmpty().WithMessage("İçerik boş olamaz.");

        }


    }
}
