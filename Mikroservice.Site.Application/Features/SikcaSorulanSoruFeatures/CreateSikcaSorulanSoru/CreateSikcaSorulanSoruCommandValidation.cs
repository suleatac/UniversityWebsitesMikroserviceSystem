using FluentValidation;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.CreateSikcaSorulanSoru
{
    public class CreateSikcaSorulanSoruCommandValidation : AbstractValidator<CreateSikcaSorulanSoruCommand>
    {
        public CreateSikcaSorulanSoruCommandValidation()
        {
            RuleFor(x => x.SiteId).GreaterThan(0);
            RuleFor(x => x.DilId).GreaterThan(0);
            RuleFor(x => x.KategoriId).GreaterThan(0);
            RuleFor(x => x.Soru).NotEmpty();
            RuleFor(x => x.Cevap).NotEmpty();
        }
    }
}
