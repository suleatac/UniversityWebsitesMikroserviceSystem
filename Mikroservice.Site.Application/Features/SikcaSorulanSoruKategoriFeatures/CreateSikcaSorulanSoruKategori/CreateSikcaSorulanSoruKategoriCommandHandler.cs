using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.CreateSikcaSorulanSoruKategori
{
    public class CreateSikcaSorulanSoruKategoriCommandHandler(
         ISikcaSorulanSoruKategoriRepository kategoriRepository,
         IUnitOfWork unitOfWork,
         IRedisCacheService redisCache
     ) : IRequestHandler<CreateSikcaSorulanSoruKategoriCommand, ServiceResult<CreateSikcaSorulanSoruKategoriResponse>>
    {
        public async Task<ServiceResult<CreateSikcaSorulanSoruKategoriResponse>> Handle(CreateSikcaSorulanSoruKategoriCommand request, CancellationToken cancellationToken)
        {
            var entity = new SikcaSorulanSoruKategori
            {
                Ad = request.Ad,
                Sira = request.Sira,
                IsDeleted = false
            };

            await kategoriRepository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache invalidation event
            var key = $"sikcaSorulanSoruKategori:list";
            await redisCache.RemoveAsync(key, cancellationToken);


            var response = new CreateSikcaSorulanSoruKategoriResponse(entity.Id);
            return ServiceResult<CreateSikcaSorulanSoruKategoriResponse>.SuccessAsCreated(response, $"/api/v1/sikcasorulansorukategoriler/{entity.Id}");
        }
    }
}
