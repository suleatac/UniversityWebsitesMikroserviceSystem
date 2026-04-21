using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.MediaFileFeatures.GetMediaFile
{
    public class GetMediaFilesQueryHandler(
          IMediaFileRepository mediaFileRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetMediaFilesQueryHandler> logger
        )
        : IRequestHandler<GetMediaFilesQuery, ServiceResult<List<MediaFile>>>
    {
        public async Task<ServiceResult<List<MediaFile>>> Handle(GetMediaFilesQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"mediafiles:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<MediaFile>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "MediaFile cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<MediaFile>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = await mediaFileRepository.GetAll().Where(b => b.SiteId == request.SiteId && b.DilId == request.DilId).ToListAsync(cancellationToken);

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation(
                "MediaFile verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);
            return ServiceResult<List<MediaFile>>.SuccessAsOK(data);
        }
    }
}
