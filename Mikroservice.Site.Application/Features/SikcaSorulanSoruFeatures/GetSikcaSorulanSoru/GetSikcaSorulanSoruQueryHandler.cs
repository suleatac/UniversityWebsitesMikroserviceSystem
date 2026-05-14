using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.SikcaSorulanSoruDtos;
using Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoru;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoru
{
    public class GetSikcaSorulanSoruQueryHandler(
          ISikcaSorulanSoruRepository sikcaSorulanSoruRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetSikcaSorulanSoruQueryHandler> logger
        )
        : IRequestHandler<GetSikcaSorulanSoruQuery, ServiceResult<List<SikcaSorulanSoruDto>>>
    {
        public async Task<ServiceResult<List<SikcaSorulanSoruDto>>> Handle(GetSikcaSorulanSoruQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"sikcasorulansoru:list:{request.SiteId}:{request.DilId}";

            // ✔ Cache kontrol
            var cached = await redisCacheService.GetListAsync<SikcaSorulanSoruDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation(
                    "SikcaSorulanSoru cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                    request.SiteId,
                    request.DilId,
                    cached.Count);

                return ServiceResult<List<SikcaSorulanSoruDto>>.SuccessAsOK(cached);
            }

            // ✔ DB'den flat veri çek
            var data = sikcaSorulanSoruRepository.GetAll()
                .Where(x =>
                    x.SiteId == request.SiteId &&
                    x.DilId == request.DilId)
                .OrderBy(x => x.Sira)
                .ToList();

            // ✔ Tree oluştur
            var tree = BuildTree(data);

            // ✔ Cache'e yaz
            await redisCacheService.SetListAsync(
                cacheKey,
                tree,
                TimeSpan.FromHours(6),
                cancellationToken);

            logger.LogInformation(
                "SikcaSorulanSoru DB'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                tree.Count);

            return ServiceResult<List<SikcaSorulanSoruDto>>.SuccessAsOK(tree);
        }

        private List<SikcaSorulanSoruDto> BuildTree(List<SikcaSorulanSoru> list, int? parentId = null)
        {
            return list
                .Where(x => x.ParentId == parentId)
                .OrderBy(x => x.Sira)
                .Select(x => new SikcaSorulanSoruDto {
                    Id = x.Id,
                    SiteId = x.SiteId,
                    DilId = x.DilId,
                    ParentId = x.ParentId,
                    Soru = x.Soru,
                    Cevap = x.Cevap,
                    Sira = x.Sira,
                    SeoUrl = x.SeoUrl,
                    Children = BuildTree(list, x.Id)
                })
                .ToList();
        }
    }
}
