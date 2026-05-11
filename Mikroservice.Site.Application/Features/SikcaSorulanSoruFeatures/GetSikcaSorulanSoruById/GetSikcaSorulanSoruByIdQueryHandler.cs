using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;
using System.Net;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoruById
{
    public class GetSikcaSorulanSoruByIdQueryHandler(
        ISikcaSorulanSoruRepository sikcaSorulanSoruRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetSikcaSorulanSoruByIdQueryHandler> logger
      )
      : IRequestHandler<GetSikcaSorulanSoruByIdQuery, ServiceResult<SikcaSorulanSoru>>
    {
        public async Task<ServiceResult<SikcaSorulanSoru>> Handle(GetSikcaSorulanSoruByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"sss:{request.Id}";

            var cached = await redisCacheService.GetAsync<SikcaSorulanSoru>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("SikcaSorulanSoru cache'den alındı. Id: {Id}", request.Id);
                return ServiceResult<SikcaSorulanSoru>.SuccessAsOK(cached);
            }

            var entity = await sikcaSorulanSoruRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("SikcaSorulanSoru bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<SikcaSorulanSoru>.Error("SikcaSorulanSoru bulunamadı", HttpStatusCode.NotFound);
            }

            await redisCacheService.SetAsync(cacheKey, entity, TimeSpan.FromHours(24), cancellationToken);

            logger.LogInformation("SikcaSorulanSoru DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<SikcaSorulanSoru>.SuccessAsOK(entity);
        }
    }
}