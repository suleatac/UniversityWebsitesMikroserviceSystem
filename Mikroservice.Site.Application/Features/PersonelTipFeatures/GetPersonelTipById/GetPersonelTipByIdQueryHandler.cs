using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;
using System.Net;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.GetPersonelTipById
{
    public class GetPersonelTipByIdQueryHandler(
        IPersonelTipRepository personelTipRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetPersonelTipByIdQueryHandler> logger
      )
      : IRequestHandler<GetPersonelTipByIdQuery, ServiceResult<PersonelTip>>
    {
        public async Task<ServiceResult<PersonelTip>> Handle(GetPersonelTipByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"personelTip:{request.Id}";

            var cached = await redisCacheService.GetAsync<PersonelTip>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("PersonelTip cache'den alındı. Id: {Id}", request.Id);
                return ServiceResult<PersonelTip>.SuccessAsOK(cached);
            }

            var entity = await personelTipRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("PersonelTip bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<PersonelTip>.Error("PersonelTip bulunamadı", HttpStatusCode.NotFound);
            }

            await redisCacheService.SetAsync(cacheKey, entity, TimeSpan.FromHours(24), cancellationToken);

            logger.LogInformation("PersonelTip DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<PersonelTip>.SuccessAsOK(entity);
        }
    }
}