using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.BannerDtos;

namespace Mikroservice.Site.Application.Features.BannerFeatures.GetPaginatedBanner
{
    public class GetPaginatedBannerQueryHandler(
        IBannerRepository bannerRepository,
        IRedisCacheService redis,
        ILogger<GetPaginatedBannerQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetPaginatedBannerQuery, ServiceResult<PaginatedResult<BannerDto>>>
    {
        public async Task<ServiceResult<PaginatedResult<BannerDto>>> Handle(
            GetPaginatedBannerQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey =
                $"banner:list:" +
                $"{request.SiteId}:{request.DilId}:" +
                $"p:{request.Page}:" +
                $"ps:{request.PageSize}:" +
                $"s:{request.Search}:" +
                $"ob:{request.OrderBy}:" +
                $"od:{request.OrderDir}";

            var cachedResult =
                await redis.GetAsync<PaginatedResult<BannerDto>>(cacheKey, cancellationToken);

            if (cachedResult is not null)
            {
                logger.LogInformation("Banner listesi cache'den getirildi");
                return ServiceResult<PaginatedResult<BannerDto>>.SuccessAsOK(cachedResult);
            }

            IQueryable<Domain.Entities.Banner> query = bannerRepository
                .GetAll()
                .Where(x => x.SiteId == request.SiteId && x.DilId == request.DilId && !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();
                query = query.Where(x =>
                    x.Baslik.ToLower().Contains(search) ||
                    x.KisaAciklama.ToLower().Contains(search) ||
                    x.IcerikMetni.ToLower().Contains(search));
            }

            query = (request.OrderBy?.ToLower(), request.OrderDir?.ToLower()) switch
            {
                ("baslik", "asc") => query.OrderBy(x => x.Baslik),
                ("baslik", "desc") => query.OrderByDescending(x => x.Baslik),
                ("sira", "asc") => query.OrderBy(x => x.Sira),
                ("sira", "desc") => query.OrderByDescending(x => x.Sira),
                ("yayimtarihi", "asc") => query.OrderBy(x => x.YayimTarihi),
                ("yayimtarihi", "desc") => query.OrderByDescending(x => x.YayimTarihi),
                _ => query.OrderByDescending(x => x.Id)
            };

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.PageSize != -1)
            {
                query = query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize);
            }

            var data = await query
                .ProjectTo<BannerDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<BannerDto>
            {
                Data = data,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            await redis.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5), cancellationToken);

            logger.LogInformation(
                "Banner listesi DB'den getirildi. TotalCount: {TotalCount}, Page: {Page}",
                totalCount, request.Page);

            return ServiceResult<PaginatedResult<BannerDto>>.SuccessAsOK(result);
        }
    }
}