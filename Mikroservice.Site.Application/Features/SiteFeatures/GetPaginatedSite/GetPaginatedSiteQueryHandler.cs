using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetPaginatedSite
{
    public class GetPaginatedSiteQueryHandler(
    ISiteRepository siteRepository,
    IRedisCacheService redis,
    ILogger<GetPaginatedSiteQueryHandler> logger,
    IMapper mapper,
       IRedisCacheService redisCache
) : IRequestHandler<GetPaginatedSiteQuery, ServiceResult<SitePaginatedResult<SiteDto>>>
    {
        public async Task<ServiceResult<SitePaginatedResult<SiteDto>>> Handle(GetPaginatedSiteQuery request, CancellationToken cancellationToken)
        {
            // Cache key'i parametrelere göre oluştur (search ve order için farklı cache'ler)
            var cacheKey = $"site:paginated:{request.Page}:{request.PageSize}:{request.Search}:{request.OrderBy}:{request.OrderDir}";
            // Tüm paginated site cache'lerini sil
            //await redisCache.RemoveByPatternAsync("site:paginated:*", cancellationToken);
            //await redisCache.RemoveAsync("site:list", cancellationToken);
            // Cache kontrolü
            var cached = await redis.GetAsync<SitePaginatedResult<SiteDto>>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("Paginated site cache'den alındı. TotalCount: {TotalCount}", cached.TotalCount);
                return ServiceResult<SitePaginatedResult<SiteDto>>.SuccessAsOK(cached);
            }

            // Veritabanından sorgu
            var query = siteRepository.GetAll()
                .Where(x => !x.IsDeleted);

            // Search uygula
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(x =>
                    x.SiteAdi.ToLower().Contains(search) ||
                    x.SiteUrl.ToLower().Contains(search) ||
                    x.SiteEPosta.ToLower().Contains(search));
            }

            // Ordering uygula
            query = (request.OrderBy?.ToLower(), request.OrderDir?.ToLower()) switch {
                ("siteadi", "asc") => query.OrderBy(x => x.SiteAdi),
                ("siteadi", "desc") => query.OrderByDescending(x => x.SiteAdi),
                ("siteurl", "asc") => query.OrderBy(x => x.SiteUrl),
                ("siteurl", "desc") => query.OrderByDescending(x => x.SiteUrl),
                ("siteeposta", "asc") => query.OrderBy(x => x.SiteEPosta),
                ("siteeposta", "desc") => query.OrderByDescending(x => x.SiteEPosta),
                _ => query.OrderByDescending(x => x.Id)
            };

            // Total count
            var totalCount = await query.CountAsync(cancellationToken);

            // Paging uygula
            var entities = request.PageSize == -1
                ? await query.ToListAsync(cancellationToken)  // "Hepsi" seçildi
                : await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

            // Map et
            var mappedData = mapper.Map<List<SiteDto>>(entities);

            // Result oluştur
            var result = new SitePaginatedResult<SiteDto> {
                Data = mappedData,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            // Cache'e yaz (sadece ilk sayfa ve search yoksa - daha efektif cache)
            if (request.Page == 1 && string.IsNullOrWhiteSpace(request.Search))
            {
                await redis.SetAsync(cacheKey, result, TimeSpan.FromMinutes(30), cancellationToken);
            }

            logger.LogInformation("Paginated site veritabanından alındı. TotalCount: {TotalCount}, Page: {Page}",
                totalCount, request.Page);

            return ServiceResult<SitePaginatedResult<SiteDto>>.SuccessAsOK(result);
        }
    }

}
