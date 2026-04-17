using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.DilFeatures.GetDil
{
    public class GetDilQueryHandler(
        IDilRepository dilRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetDilQueryHandler> logger
      )
      : IRequestHandler<GetDilQuery, ServiceResult<List<Dil>>>
    {
        public async Task<ServiceResult<List<Dil>>> Handle(GetDilQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "dil:list";

            // ✔ Cache kontrol
            var cached = await redisCacheService.GetListAsync<Dil>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("Dil cache'den alındı. Count:{count}", cached.Count);
                return ServiceResult<List<Dil>>.SuccessAsOK(cached);
            }

            // ✔ DB'den flat veri çek
            var data = dilRepository.GetAll().ToList();
            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);


            logger.LogInformation("Dil DB'den alındı. Count:{count}", data.Count);

            return ServiceResult<List<Dil>>.SuccessAsOK(data);
        }

      
    }
}
