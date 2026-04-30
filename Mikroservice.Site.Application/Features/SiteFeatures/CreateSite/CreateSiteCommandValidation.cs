using FluentValidation;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteFeatures.CreateSite
{
    public class CreateSiteCommandValidation : AbstractValidator<CreateSiteCommand>
    {
        public CreateSiteCommandValidation(ISiteRepository siteRepository)
        {
            RuleFor(x => x.SiteAdi).NotEmpty().MaximumLength(200).WithMessage("Site adı boş olamaz ve en fazla 200 karakter olabilir.");

            RuleFor(x => x.SiteUrl)
            .NotEmpty().WithMessage("Geçerli bir URL giriniz.")
            .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _)
                    || Uri.TryCreate("http://" + x, UriKind.Absolute, out _))
            .WithMessage("Geçerli bir URL giriniz.");

            RuleFor(x => x.SiteAlanAdi)
                .NotEmpty().WithMessage("Site alan adı boş olamaz.");

            RuleFor(x => x.SiteEPosta)
                .EmailAddress().WithMessage("Site mail adresi türü düzgün olmalı.");

            RuleFor(x => x.SiteEPostaPort)
                .GreaterThan(0).WithMessage("SiteEPostaPort sıfırdan büyük olmalı.");

            // 🔥 unique domain
            //RuleFor(x => x.SiteAlanAdi)
            //    .MustAsync(async (alan, ct) =>
            //        !await siteRepository.AnyAsync(x => x.SiteAlanAdi == alan, ct))
            //    .WithMessage("Bu domain zaten kullanılıyor.");
        }
    }
}
