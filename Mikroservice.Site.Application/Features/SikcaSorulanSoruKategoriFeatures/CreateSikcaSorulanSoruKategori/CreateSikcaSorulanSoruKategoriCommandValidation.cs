using FluentValidation;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.CreateSikcaSorulanSoruKategori
{
    public class CreateSikcaSorulanSoruKategoriCommandValidation : AbstractValidator<CreateSikcaSorulanSoruKategoriCommand>
    {
        public CreateSikcaSorulanSoruKategoriCommandValidation()
        {
            RuleFor(x => x.Ad).NotEmpty();
        }
    }
}
