using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.GetPersonelTip
{
    public class GetPersonelTipQueryHandler(
        IPersonelTipRepository personelTipRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetPersonelTipQueryHandler> logger
      )
      : IRequestHandler<GetPersonelTipQuery, ServiceResult<List<PersonelTip>>>
    {
        public async Task<ServiceResult<List<PersonelTip>>> Handle(GetPersonelTipQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "personelTip:list";

            // ✔ Cache kontrol
            var cached = await redisCacheService.GetListAsync<PersonelTip>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("PersonelTip cache'den alındı. Count:{count}", cached.Count);
                return ServiceResult<List<PersonelTip>>.SuccessAsOK(cached);
            }

            // ✔ DB'den flat veri çek
            var data = personelTipRepository.GetAll().ToList();
            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);


            logger.LogInformation("PersonelTip DB'den alındı. Count:{count}", data.Count);

            return ServiceResult<List<PersonelTip>>.SuccessAsOK(data);
        }


    }
}
