using FluentValidation;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.UpdateSikcaSorulanSoru
{
    public class UpdateSikcaSorulanSoruCommandValidation : AbstractValidator<UpdateSikcaSorulanSoruCommand>
    {
        public UpdateSikcaSorulanSoruCommandValidation()
        {
            RuleFor(x => x.SiteId).GreaterThan(0);
            RuleFor(x => x.DilId).GreaterThan(0);
            RuleFor(x => x.KategoriId).GreaterThan(0);
            RuleFor(x => x.Soru).NotEmpty();
            RuleFor(x => x.Cevap).NotEmpty();
        }
    }
}
