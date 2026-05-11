using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;
using System.Net;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.GetSikcaSorulanSoruKategoriById
{
    public class GetSikcaSorulanSoruKategoriByIdQueryHandler(
        ISikcaSorulanSoruKategoriRepository sikcaSorulanSoruKategoriRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetSikcaSorulanSoruKategoriByIdQueryHandler> logger
      )
      : IRequestHandler<GetSikcaSorulanSoruKategoriByIdQuery, ServiceResult<SikcaSorulanSoruKategori>>
    {
        public async Task<ServiceResult<SikcaSorulanSoruKategori>> Handle(GetSikcaSorulanSoruKategoriByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"sss-kategori:{request.Id}";

            var cached = await redisCacheService.GetAsync<SikcaSorulanSoruKategori>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("SikcaSorulanSoruKategori cache'den alındı. Id: {Id}", request.Id);
                return ServiceResult<SikcaSorulanSoruKategori>.SuccessAsOK(cached);
            }

            var entity = await sikcaSorulanSoruKategoriRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("SikcaSorulanSoruKategori bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<SikcaSorulanSoruKategori>.Error("SikcaSorulanSoruKategori bulunamadı", HttpStatusCode.NotFound);
            }

            await redisCacheService.SetAsync(cacheKey, entity, TimeSpan.FromHours(24), cancellationToken);

            logger.LogInformation("SikcaSorulanSoruKategori DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<SikcaSorulanSoruKategori>.SuccessAsOK(entity);
        }
    }
}