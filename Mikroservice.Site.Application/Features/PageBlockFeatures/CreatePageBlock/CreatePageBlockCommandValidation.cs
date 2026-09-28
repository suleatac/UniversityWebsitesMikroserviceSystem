using FluentValidation;
using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Application.Features.PageBlockFeatures.CreatePageBlock
{
    public class CreatePageBlockCommandValidation : AbstractValidator<CreatePageBlockCommand>
    {
        public CreatePageBlockCommandValidation()
        {
            RuleFor(x => x.PageSectionId)
                .GreaterThan(0).WithMessage("Geçerli bir PageSectionId girilmelidir.");

            RuleFor(x => x.ContentType)
                .NotEmpty().WithMessage("İçerik tipi seçilmelidir.")
                .Must(BlockContentType.IsValid).WithMessage("Geçersiz içerik tipi. Text / Video / Image / Carousel olmalıdır.");

            // 🔹 Icerik tipine gore kosullu alan dogrulamalari
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Metin içeriği boş olamaz.")
                .When(x => string.Equals(x.ContentType, BlockContentType.Text, StringComparison.OrdinalIgnoreCase));

            RuleFor(x => x.VideoUrl)
                .NotEmpty().WithMessage("Video URL boş olamaz.")
                .When(x => string.Equals(x.ContentType, BlockContentType.Video, StringComparison.OrdinalIgnoreCase));

            RuleFor(x => x.VideoType)
                .Must(VideoTuru.IsValid).WithMessage("Video tipi YouTube veya Local olmalıdır.")
                .When(x => string.Equals(x.ContentType, BlockContentType.Video, StringComparison.OrdinalIgnoreCase));

            RuleFor(x => x.BackgroundImageUrl)
                .NotEmpty().WithMessage("Görsel adresi boş olamaz.")
                .When(x => string.Equals(x.ContentType, BlockContentType.Image, StringComparison.OrdinalIgnoreCase));

            RuleFor(x => x.Content)
                .MaximumLength(20000).WithMessage("İçerik metni en fazla 20000 karakter olabilir.");

            RuleFor(x => x.VideoUrl)
                .MaximumLength(500).WithMessage("Video URL en fazla 500 karakter olabilir.");

            RuleFor(x => x.BackgroundImageUrl)
                .MaximumLength(500).WithMessage("Arka plan görseli en fazla 500 karakter olabilir.");

            RuleFor(x => x.BackgroundColor)
                .MaximumLength(50).WithMessage("Arka plan rengi en fazla 50 karakter olabilir.");

            RuleFor(x => x.ColumnSize)
                .InclusiveBetween(1, 12).WithMessage("Kolon genişliği 1 ile 12 arasında olmalıdır.");

            RuleFor(x => x.RowNumber)
                .GreaterThanOrEqualTo(0).WithMessage("Satır numarası 0 veya daha büyük olmalıdır.");

            RuleFor(x => x.Medias)
                .Must(m => m.Count > 0).WithMessage("Carousel için en az bir slayt (resim) gereklidir.")
                .When(x => string.Equals(x.ContentType, BlockContentType.Carousel, StringComparison.OrdinalIgnoreCase));

            RuleForEach(x => x.Medias)
                .ChildRules(media => media.RuleFor(m => m.ResimUrl)
                    .NotEmpty().WithMessage("Carousel slayt resim adresi boş olamaz.")
                    .MaximumLength(500).WithMessage("Carousel slayt resim adresi en fazla 500 karakter olabilir."));
        }
    }
}
