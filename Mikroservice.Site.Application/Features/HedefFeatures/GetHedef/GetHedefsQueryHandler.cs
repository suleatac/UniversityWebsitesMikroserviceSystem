using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.HedefFeatures.GetHedef
{
    public class GetHedefsQueryHandler(
        IHedefRepository hedefRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetHedefsQueryHandler> logger
      )
      : IRequestHandler<GetHedefsQuery, ServiceResult<List<Hedef>>>
    {
        public async Task<ServiceResult<List<Hedef>>> Handle(GetHedefsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "hedef:list";

            // ✔ Cache kontrol
            var cached = await redisCacheService.GetListAsync<Hedef>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("Hedef cache'den alındı. Count:{count}", cached.Count);
                return ServiceResult<List<Hedef>>.SuccessAsOK(cached);
            }

            // ✔ DB'den flat veri çek
            var data = hedefRepository.GetAll().ToList();
            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);


            logger.LogInformation("Hedef DB'den alındı. Count:{count}", data.Count);

            return ServiceResult<List<Hedef>>.SuccessAsOK(data);
        }


    }
}
