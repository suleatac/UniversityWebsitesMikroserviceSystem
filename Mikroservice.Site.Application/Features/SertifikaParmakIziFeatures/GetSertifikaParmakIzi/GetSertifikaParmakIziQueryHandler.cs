using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.GetSertifikaParmakIzi
{
    public class GetSertifikaParmakIziQueryHandler(
          ISertifikaParmakIziRepository sertifikaParmakIziRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetSertifikaParmakIziQueryHandler> logger
        )
        : IRequestHandler<GetSertifikaParmakIziQuery, ServiceResult<List<SertifikaParmakIzi>>>
    {
        public async Task<ServiceResult<List<SertifikaParmakIzi>>> Handle(GetSertifikaParmakIziQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"sertifikaparmakizi:list";
            var cached = await redisCacheService.GetListAsync<SertifikaParmakIzi>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                    "Sertifika parmak izi verisi cacheden alındı. Count:{count}",
                    cached.Count);
                return ServiceResult<List<SertifikaParmakIzi>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = await sertifikaParmakIziRepository.GetAll().ToListAsync(cancellationToken);

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation(
                "Sertifika parmak izi verisi veritabanından alındı. Count:{count}",
                data.Count);
            return ServiceResult<List<SertifikaParmakIzi>>.SuccessAsOK(data);
        }
    }
}
