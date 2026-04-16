using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BannerFeatures.GetBanners
{
    public class GetBannersQueryHandler(
          IBannerRepository bannerRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetBannersQueryHandler> logger
        )
        : IRequestHandler<GetBannersQuery, ServiceResult<List<Banner>>>
    {
        public async Task<ServiceResult<List<Banner>>> Handle(GetBannersQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"banners:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<Banner>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "Banner cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<Banner>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = bannerRepository.GetAll().Where(b => b.SiteId == request.SiteId && b.DilId == request.DilId).OrderBy(x => x.Sira).ToList();

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation(
                "Banner verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);
            return ServiceResult<List<Banner>>.SuccessAsOK(data);
        }
    }
}
