using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.DTOs;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BirimFeatures.GetBirim
{
    public class GetBirimQueryHandler(
        IBirimRepository birimRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetBirimQueryHandler> logger
      )
      : IRequestHandler<GetBirimQuery, ServiceResult<List<BirimDto>>>
    {
        public async Task<ServiceResult<List<BirimDto>>> Handle(GetBirimQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "birim:list";

            // ✔ Cache kontrol
            var cached = await redisCacheService.GetListAsync<BirimDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("Birim cache'den alındı. Count:{count}", cached.Count);
                return ServiceResult<List<BirimDto>>.SuccessAsOK(cached);
            }

            // ✔ DB'den flat veri çek
            var data = birimRepository.GetAll().OrderBy(x => x.Sira).ToList();

            // ✔ Tree'ye çevir
            var tree = BuildTree(data);

            // ✔ Cache'e DTO olarak yaz
            await redisCacheService.SetListAsync(
                cacheKey,
                tree,
                TimeSpan.FromHours(24),
                cancellationToken);

            logger.LogInformation("Birim DB'den alındı. Count:{count}", tree.Count);

            return ServiceResult<List<BirimDto>>.SuccessAsOK(tree);
        }

        private List<BirimDto> BuildTree(List<Birim> list, int? parentId = null)
        {
            return list
                .Where(x => x.ParentId == parentId)
                .Select(x => new BirimDto {
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
