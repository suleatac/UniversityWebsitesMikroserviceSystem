using FluentValidation;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.UpdateSikcaSorulanSoruKategori
{
    public class UpdateSikcaSorulanSoruKategoriCommandValidation : AbstractValidator<UpdateSikcaSorulanSoruKategoriCommand>
    {
        public UpdateSikcaSorulanSoruKategoriCommandValidation()
        {
            RuleFor(x => x.Ad).NotEmpty();
        }
    }
}
