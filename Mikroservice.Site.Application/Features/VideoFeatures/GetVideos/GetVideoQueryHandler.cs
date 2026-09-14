using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.VideoDtos;

namespace Mikroservice.Site.Application.Features.VideoFeatures.GetVideos
{
    public class GetVideoQueryHandler(
      IVideoRepository videoRepository,
      IRedisCacheService redis,
      ILogger<GetVideoQueryHandler> logger,
      IMapper mapper
  ) : IRequestHandler<GetVideosQuery, ServiceResult<List<VideoDto>>>
    {
        public async Task<ServiceResult<List<VideoDto>>> Handle(
            GetVideosQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"video:list:{request.SiteId}:{request.DilId}";

            var cached = await redis.GetListAsync<VideoDto>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                logger.LogInformation("Video cache'den alındı");
                return ServiceResult<List<VideoDto>>.SuccessAsOK(cached);
            }

            // Yoksa veritabanından çek
            var data = await videoRepository.GetBySiteAndLanguageAsync(request.SiteId, request.DilId, cancellationToken);

            //Loglama
            logger.LogInformation(
                "Video verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);

            var mappedData = mapper.Map<List<VideoDto>>(data);

            await redis.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(12), cancellationToken);

            var dtoData = mapper.Map<List<VideoDto>>(data);

            return ServiceResult<List<VideoDto>>.SuccessAsOK(dtoData);
        }
    }
}
