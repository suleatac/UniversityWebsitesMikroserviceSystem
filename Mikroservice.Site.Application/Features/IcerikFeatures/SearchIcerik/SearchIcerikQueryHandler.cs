using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.IcerikDtos;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Application.Features.IcerikFeatures.SearchIcerik
{
    public class SearchIcerikQueryHandler(
        IIcerikRepository icerikRepository,
        IRedisCacheService redis,
        ILogger<SearchIcerikQueryHandler> logger
    ) : IRequestHandler<SearchIcerikQuery, ServiceResult<PaginatedResult<IcerikSearchDto>>>
    {
        public async Task<ServiceResult<PaginatedResult<IcerikSearchDto>>> Handle(
            SearchIcerikQuery request,
            CancellationToken cancellationToken)
        {
            if (request.SiteId <= 0 || request.DilId <= 0)
            {
                return ServiceResult<PaginatedResult<IcerikSearchDto>>.Error(
                    "Gecersiz SiteId veya DilId", System.Net.HttpStatusCode.BadRequest);
            }

            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize is <= 0 or > 100 ? 5 : request.PageSize;

            // Query-specific cache key
            var cacheKey =
                $"icerik:search:" +
                $"{request.SiteId}:{request.DilId}:" +
                $"t:{request.Tip}:" +
                $"p:{page}:" +
                $"ps:{pageSize}:" +
                $"s:{request.Search?.Trim().ToLowerInvariant()}";

            var cachedResult =
                await redis.GetAsync<PaginatedResult<IcerikSearchDto>>(cacheKey, cancellationToken);

            if (cachedResult is not null)
            {
                logger.LogInformation("Icerik arama sonucu cache'den getirildi");

                return ServiceResult<PaginatedResult<IcerikSearchDto>>
                    .SuccessAsOK(cachedResult);
            }

            // Base query: soft-delete filteri repository uzerindeki query filter ile otomatik uygulanir.
            IQueryable<Icerik> query = icerikRepository
                .GetAll()
                .Where(x => x.SiteId == request.SiteId && x.DilId == request.DilId);

            // Arama yapilabilecek icerik turleri (Menu ve Banner haric).
            // IcerikTip degerleri: 1=Haber, 2=Duyuru, 3=Bilgi, 4=Etkinlik, 5=Video
            query = request.Tip switch {
                (int)IcerikTip.Haber => query.Where(x => x is Haber),
                (int)IcerikTip.Duyuru => query.Where(x => x is Duyuru),
                (int)IcerikTip.Bilgi => query.Where(x => x is Bilgi),
                (int)IcerikTip.Etkinlik => query.Where(x => x is Etkinlik),
                (int)IcerikTip.Video => query.Where(x => x is Video),
                _ => query.Where(x =>
                    x is Haber || x is Duyuru || x is Bilgi || x is Etkinlik || x is Video)
            };

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                query = query.Where(x =>
                    x.Baslik.ToLower().Contains(search) ||
                    x.KisaAciklama!.ToLower().Contains(search) ||
                    x.IcerikMetni!.ToLower().Contains(search));
            }

            // Total count
            var totalCount = await query.CountAsync(cancellationToken);

            // Pagination + DTO projection (Tip, discriminator'a gore hesaplanir)
            var data = await query
                .OrderByDescending(x => x.YayimTarihi)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new IcerikSearchDto {
                    Id = x.Id,
                    Tip = x is Haber
                        ? IcerikTip.Haber
                        : x is Duyuru
                            ? IcerikTip.Duyuru
                            : x is Bilgi
                                ? IcerikTip.Bilgi
                                : x is Etkinlik
                                    ? IcerikTip.Etkinlik
                                    : IcerikTip.Video,
                    SiteId = x.SiteId,
                    DilId = x.DilId,
                    PageTypeId = x.PageTypeId,
                    Baslik = x.Baslik,
                    KisaAciklama = x.KisaAciklama,
                    ResimUrl = x.ResimUrl,
                    Link = x.Link,
                    SeoUrl = x.SeoUrl,
                    YayimTarihi = x.YayimTarihi,
                    PageType = new DTOs.PageTypeDtos.PageTypeDto {
                        Id = x.PageType.Id,
                        PageTypeKind = x.PageType.PageTypeKind,
                        Slug = x.PageType.Slug,
                        TemplateId = x.PageType.TemplateId,
                        DilId = x.PageType.DilId,
                        ViewName = x.PageType.ViewName,
                        IsHomePage = x.PageType.IsHomePage
                    }
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<IcerikSearchDto> {
                Data = data,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            // Cache save (5 dk)
            await redis.SetAsync(
                cacheKey,
                result,
                TimeSpan.FromMinutes(5),
                cancellationToken);

            logger.LogInformation(
                "Icerik aramasi tamamlandi. Search: {Search}, TotalCount: {TotalCount}, Page: {Page}",
                request.Search,
                totalCount,
                page);

            return ServiceResult<PaginatedResult<IcerikSearchDto>>
                .SuccessAsOK(result);
        }
    }
}
