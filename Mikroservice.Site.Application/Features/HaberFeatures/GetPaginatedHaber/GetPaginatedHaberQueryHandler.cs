using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.HaberDtos;

namespace Mikroservice.Site.Application.Features.HaberFeatures.GetPaginatedHaber
{
    public class GetPaginatedHaberQueryHandler(
        IHaberRepository haberRepository,
        IRedisCacheService redis,
        ILogger<GetPaginatedHaberQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetPaginatedHaberQuery, ServiceResult<PaginatedResult<HaberDto>>>
    {
        public async Task<ServiceResult<PaginatedResult<HaberDto>>> Handle(
            GetPaginatedHaberQuery request,
            CancellationToken cancellationToken)
        {
            // Query-specific cache key
            var cacheKey =
                $"haber:list:" +
                $"{request.SiteId}:{request.DilId}:" +
                $"p:{request.Page}:" +
                $"ps:{request.PageSize}:" +
                $"s:{request.Search}:" +
                $"ob:{request.OrderBy}:" +
                $"od:{request.OrderDir}";

            // Cache kontrolü
            var cachedResult =
                await redis.GetAsync<PaginatedResult<HaberDto>>(cacheKey, cancellationToken);

            if (cachedResult is not null)
            {
                logger.LogInformation("Haber listesi cache'den getirildi");

                return ServiceResult<PaginatedResult<HaberDto>>
                    .SuccessAsOK(cachedResult);
            }

            // Base query - silinmemiş kayıtları getir
            IQueryable<Domain.Entities.Haber> query = haberRepository
                .GetAll()
                .Where(x => !x.IsDeleted);

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                query = query.Where(x =>
                    x.Baslik.ToLower().Contains(search) ||
                    x.KisaAciklama.ToLower().Contains(search) ||
                    x.IcerikMetni.ToLower().Contains(search));
            }

            // Ordering
            query = (request.OrderBy?.ToLower(), request.OrderDir?.ToLower()) switch {
                ("baslik", "asc") =>
                    query.OrderBy(x => x.Baslik),

                ("baslik", "desc") =>
                    query.OrderByDescending(x => x.Baslik),

                ("yayimtarihi", "asc") =>
                    query.OrderBy(x => x.YayimTarihi),

                ("yayimtarihi", "desc") =>
                    query.OrderByDescending(x => x.YayimTarihi),

                ("baslamatarihi", "asc") =>
                    query.OrderBy(x => x.BaslamaTarihi),

                ("baslamatarihi", "desc") =>
                    query.OrderByDescending(x => x.BaslamaTarihi),

                ("bitistarihi", "asc") =>
                    query.OrderBy(x => x.BitisTarihi),

                ("bitistarihi", "desc") =>
                    query.OrderByDescending(x => x.BitisTarihi),

                _ =>
                    query.OrderByDescending(x => x.Id)
            };

            // Total count
            var totalCount = await query.CountAsync(cancellationToken);

            // Pagination
            if (request.PageSize != -1)
            {
                query = query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize);
            }

            // DTO projection
            var data = await query
                .ProjectTo<HaberDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            // Result
            var result = new PaginatedResult<HaberDto> {
                Data = data,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            // Cache save (5 dk)
            await redis.SetAsync(
                cacheKey,
                result,
                TimeSpan.FromMinutes(5),
                cancellationToken);

            logger.LogInformation(
                "Haber listesi DB'den getirildi. TotalCount: {TotalCount}, Page: {Page}",
                totalCount,
                request.Page);

            return ServiceResult<PaginatedResult<HaberDto>>
                .SuccessAsOK(result);
        }
    }
}
