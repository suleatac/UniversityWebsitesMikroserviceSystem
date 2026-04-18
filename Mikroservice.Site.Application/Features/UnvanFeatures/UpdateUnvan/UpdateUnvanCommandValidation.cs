using FluentValidation;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Features.UnvanFeatures.CreateUnvan;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.UpdateUnvan
{
    public class UpdateUnvanCommandValidation : AbstractValidator<UpdateUnvanCommand>
    {
        public UpdateUnvanCommandValidation(IUnvanRepository repository)
        {
            RuleFor(x => x.TipId).GreaterThan(0);

            RuleFor(x => x.Ad)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.KisaAd)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Sira)
                .GreaterThanOrEqualTo(0);

            // 🔥 duplicate kontrol
            RuleFor(x => x.Ad)
                .MustAsync(async (ad, cancellationToken) =>
                    !await repository.AnyByAdAsync(ad, cancellationToken))
                .WithMessage("Bu ünvan zaten mevcut.");
        }
    }
}
