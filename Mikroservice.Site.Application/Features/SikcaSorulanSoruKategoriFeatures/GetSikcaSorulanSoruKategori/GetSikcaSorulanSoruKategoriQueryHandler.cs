using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.GetSikcaSorulanSoruKategori
{
    public class GetSikcaSorulanSoruKategoriQueryHandler(
     ISikcaSorulanSoruKategoriRepository kategoriRepository,
     IRedisCacheService redisCacheService,
     ILogger<GetSikcaSorulanSoruKategoriQueryHandler> logger
 ) : IRequestHandler<GetSikcaSorulanSoruKategoriQuery, ServiceResult<List<SikcaSorulanSoruKategori>>>
    {
        public async Task<ServiceResult<List<SikcaSorulanSoruKategori>>> Handle(
            GetSikcaSorulanSoruKategoriQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = "sikcaSorulanSoruKategori:list";

            // 🔹 Cache kontrol
            var cached = await redisCacheService.GetListAsync<SikcaSorulanSoruKategori>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("Kategori cache'den alındı. Count:{count}", cached.Count);
                return ServiceResult<List<SikcaSorulanSoruKategori>>.SuccessAsOK(cached);
            }

            // 🔹 DB
            var data = kategoriRepository.GetAll()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Sira)
                .ToList();

            // 🔹 Cache
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            logger.LogInformation("Kategori DB'den alındı. Count:{count}", data.Count);

            return ServiceResult<List<SikcaSorulanSoruKategori>>.SuccessAsOK(data);
        }
    }
}
