using FluentValidation;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteFeatures.CreateSite
{
    public class CreateSiteCommandValidation : AbstractValidator<CreateSiteCommand>
    {
        public CreateSiteCommandValidation(ISiteRepository siteRepository)
        {
            RuleFor(x => x.SiteAdi).NotEmpty().MaximumLength(200);

            RuleFor(x => x.SiteUrl)
                .NotEmpty()
                .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
                .WithMessage("Geçerli bir URL giriniz.");

            RuleFor(x => x.SiteAlanAdi)
                .NotEmpty();

            RuleFor(x => x.SiteEPosta)
                .EmailAddress();

            RuleFor(x => x.SiteEPostaPort)
                .GreaterThan(0);

            // 🔥 unique domain
            //RuleFor(x => x.SiteAlanAdi)
            //    .MustAsync(async (alan, ct) =>
            //        !await siteRepository.AnyAsync(x => x.SiteAlanAdi == alan, ct))
            //    .WithMessage("Bu domain zaten kullanılıyor.");
        }
    }
}
