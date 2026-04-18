using FluentValidation;

namespace Mikroservice.Site.Application.Features.VideoFeatures.UpdateVideo
{
    public class UpdateVideoCommandValidation
     : AbstractValidator<UpdateVideoCommand>
    {
        public UpdateVideoCommandValidation()
        {
            RuleFor(x => x.SiteId).GreaterThan(0);
            RuleFor(x => x.DilId).GreaterThan(0);

            RuleFor(x => x.Baslik)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.KisaAciklama)
                .NotEmpty();

            RuleFor(x => x.YayimTarihi)
                .LessThanOrEqualTo(DateTime.Now.AddYears(1));

            RuleFor(x => x.VideoUrl)
                .Must(x => string.IsNullOrEmpty(x) || Uri.IsWellFormedUriString(x, UriKind.Absolute))
                .WithMessage("Geçerli video URL giriniz.");
        }
    }
}
