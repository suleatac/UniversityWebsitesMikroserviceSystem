using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.VideoFeatures.GetVideos
{
    public class GetVideoQueryHandler(
      IVideoRepository repository,
      IRedisCacheService redis,
      ILogger<GetVideoQueryHandler> logger
  ) : IRequestHandler<GetVideosQuery, ServiceResult<List<Video>>>
    {
        public async Task<ServiceResult<List<Video>>> Handle(
            GetVideosQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"video:list:{request.SiteId}:{request.DilId}";

            var cached = await redis.GetListAsync<Video>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                logger.LogInformation("Video cache'den alındı");
                return ServiceResult<List<Video>>.SuccessAsOK(cached);
            }

            var now = DateTime.Now;

            var data = repository.GetAll()
                .Where(x =>
                    x.SiteId == request.SiteId &&
                    x.DilId == request.DilId &&
                    !x.IsDeleted &&
                    (x.BaslamaTarihi == null || x.BaslamaTarihi <= now) &&
                    (x.BitisTarihi == null || x.BitisTarihi >= now))
                .OrderByDescending(x => x.YayimTarihi)
                .ToList();

            await redis.SetListAsync(cacheKey, data, TimeSpan.FromHours(12), cancellationToken);

            return ServiceResult<List<Video>>.SuccessAsOK(data);
        }
    }
}
