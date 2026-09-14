using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.BannerDtos;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BannerFeatures.GetBanners
{
    public class GetBannersQueryHandler(
          IBannerRepository bannerRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetBannersQueryHandler> logger,
          IMapper mapper
        )
        : IRequestHandler<GetBannersQuery, ServiceResult<List<BannerDto>>>
    {
        public async Task<ServiceResult<List<BannerDto>>> Handle(GetBannersQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"banners:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<BannerDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "Banner cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<BannerDto>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = await bannerRepository.GetBySiteAndLanguageAsync(request.SiteId, request.DilId, cancellationToken);

            //Loglama
            logger.LogInformation(
                "Banner verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);

            var mappedData = mapper.Map<List<BannerDto>>(data);

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(24), cancellationToken);

         
            return ServiceResult<List<BannerDto>>.SuccessAsOK(mappedData);
        }
    }
}
