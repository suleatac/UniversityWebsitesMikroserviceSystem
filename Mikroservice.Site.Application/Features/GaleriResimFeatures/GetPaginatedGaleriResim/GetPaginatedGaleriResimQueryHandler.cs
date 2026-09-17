using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.GetPaginatedGaleriResim
{
    public class GetPaginatedGaleriResimQueryHandler(
        IGaleriResimRepository galeriResimRepository,
        IRedisCacheService redis,
        ILogger<GetPaginatedGaleriResimQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetPaginatedGaleriResimQuery, ServiceResult<PaginatedResult<GaleriResimDto>>>
    {
        public async Task<ServiceResult<PaginatedResult<GaleriResimDto>>> Handle(
            GetPaginatedGaleriResimQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey =
                $"galeriresim:list:" +
                $"{request.SiteId}:{request.DilId}:" +
                $"p:{request.Page}:" +
                $"ps:{request.PageSize}:" +
                $"s:{request.Search}:" +
                $"k:{request.Kategori}:" +
                $"ob:{request.OrderBy}:" +
                $"od:{request.OrderDir}";

            var cachedResult =
                await redis.GetAsync<PaginatedResult<GaleriResimDto>>(cacheKey, cancellationToken);

            if (cachedResult is not null)
            {
                logger.LogInformation("GaleriResim listesi cache'den getirildi");
                return ServiceResult<PaginatedResult<GaleriResimDto>>.SuccessAsOK(cachedResult);
            }

            IQueryable<Domain.Entities.GaleriResim> query = galeriResimRepository
                .GetAll()
                .Where(x => x.SiteId == request.SiteId && x.DilId == request.DilId && !x.IsDeleted);

            // Kategori filtresi: tam eslesme (buyuk/kucuk harf duyarsiz).
            if (!string.IsNullOrWhiteSpace(request.Kategori))
            {
                var kategori = request.Kategori.Trim().ToLower();
                query = query.Where(x => x.Kategori!.ToLower() == kategori);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();
                query = query.Where(x =>
                    x.Baslik.ToLower().Contains(search) ||
                    x.KisaAciklama!.ToLower().Contains(search) ||
                    x.Kategori!.ToLower().Contains(search));
            }

            query = (request.OrderBy?.ToLower(), request.OrderDir?.ToLower()) switch
            {
                ("baslik", "asc") => query.OrderBy(x => x.Baslik),
                ("baslik", "desc") => query.OrderByDescending(x => x.Baslik),
                ("kategori", "asc") => query.OrderBy(x => x.Kategori),
                ("kategori", "desc") => query.OrderByDescending(x => x.Kategori),
                ("yayimtarihi", "asc") => query.OrderBy(x => x.YayimTarihi),
                ("yayimtarihi", "desc") => query.OrderByDescending(x => x.YayimTarihi),
                ("sira", "asc") => query.OrderBy(x => x.Sira).ThenByDescending(x => x.YayimTarihi),
                ("sira", "desc") => query.OrderByDescending(x => x.Sira).ThenByDescending(x => x.YayimTarihi),
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
                .ProjectTo<GaleriResimDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<GaleriResimDto>
            {
                Data = data,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            await redis.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5), cancellationToken);

            logger.LogInformation(
                "GaleriResim listesi DB'den getirildi. TotalCount: {TotalCount}, Page: {Page}",
                totalCount, request.Page);

            return ServiceResult<PaginatedResult<GaleriResimDto>>.SuccessAsOK(result);
        }
    }
}
