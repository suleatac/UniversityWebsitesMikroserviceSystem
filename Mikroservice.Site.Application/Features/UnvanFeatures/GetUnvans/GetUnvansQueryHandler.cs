using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.UnvanDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.GetUnvans
{
    public class GetUnvanQueryHandler(
        IUnvanRepository unvanRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetUnvanQueryHandler> logger
      )
      : IRequestHandler<GetUnvanQuery, ServiceResult<List<UnvanDto>>>
    {
        public async Task<ServiceResult<List<UnvanDto>>> Handle(GetUnvanQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "unvan:list";

            // ✔ Cache kontrol
            var cached = await redisCacheService.GetListAsync<UnvanDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("Unvan cache'den alındı. Count:{count}", cached.Count);
                return ServiceResult<List<UnvanDto>>.SuccessAsOK(cached);
            }

            // ✔ DB'den flat veri çek
            var data = unvanRepository.GetAll().OrderBy(x => x.Sira).ToList();

            // ✔ Tree'ye çevir
            var tree = BuildTree(data);

            // ✔ Cache'e DTO olarak yaz
            await redisCacheService.SetListAsync(
                cacheKey,
                tree,
                TimeSpan.FromHours(24),
                cancellationToken);

            logger.LogInformation("Unvan DB'den alındı. Count:{count}", tree.Count);

            return ServiceResult<List<UnvanDto>>.SuccessAsOK(tree);
        }

        private List<UnvanDto> BuildTree(List<Unvan> list, int? parentId = null)
        {
            return list
                .Where(x => x.ParentId == parentId)
                .Select(x => new UnvanDto {
                    Id = x.Id,
                    Ad = x.Ad,
                    Sira = x.Sira,
                    ParentId = x.ParentId,
                    Children = BuildTree(list, x.Id)
                })
                .ToList();
        }
    }
}
