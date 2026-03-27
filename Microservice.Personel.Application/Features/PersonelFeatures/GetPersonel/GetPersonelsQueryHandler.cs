using AutoMapper;
using MediatR;
using Microservice.Personel.Application.Contracts.IRepositories;
using Microservice.Shared;
using Microservice.Shared.RedisCacheItems;
using StackExchange.Redis;

namespace Microservice.Personel.Application.Features.PersonelFeatures.GetPersonel
{
    public class GetPersonelsQueryHandler(IPersonelRepository personelRepository, IRedisCacheService redisCacheService) : IRequestHandler<GetPersonelsQuery, ServiceResult<List<Microservice.Personel.Domain.Entities.Personel>>>
    {
  
        public async Task<ServiceResult<List<Domain.Entities.Personel>>> Handle(GetPersonelsQuery request, CancellationToken cancellationToken)
        {
      
            // Önce cache'e bak
            var cacheKey = "list:personels";
            var cached = await redisCacheService.GetListAsync<Domain.Entities.Personel>(cacheKey, cancellationToken);
            if (cached is not null)
                return ServiceResult<List<Domain.Entities.Personel>>.SuccessAsOK(cached);

            // Yoksa veritabanından çek
            var data =  personelRepository.GetAll().ToList();
            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);
            return ServiceResult<List<Domain.Entities.Personel>>.SuccessAsOK(data);

        }
    }
}
