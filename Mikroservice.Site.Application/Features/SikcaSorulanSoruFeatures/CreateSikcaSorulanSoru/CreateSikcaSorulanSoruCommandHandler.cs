using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.CreateSikcaSorulanSoru
{
    public class CreateSikcaSorulanSoruCommandHandler(
           ISikcaSorulanSoruRepository soruRepository,
           IUnitOfWork unitOfWork,
           IRedisCacheService redisCache
       ) : IRequestHandler<CreateSikcaSorulanSoruCommand, ServiceResult<CreateSikcaSorulanSoruResponse>>
    {
        public async Task<ServiceResult<CreateSikcaSorulanSoruResponse>> Handle(CreateSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
        {
            var entity = new SikcaSorulanSoru
            {
                SiteId = request.SiteId,
                DilId = request.DilId,
                ParentId = request.ParentId,

                Soru = request.Soru,
                Cevap = request.Cevap,

                Sira = request.Sira,
                SeoUrl = request.SeoUrl
            };

            await soruRepository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache silme işlemi
            var key = $"sikcasorulansoru:list:{request.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            var response = new CreateSikcaSorulanSoruResponse(entity.Id);
            return ServiceResult<CreateSikcaSorulanSoruResponse>
            .SuccessAsCreated(response, $"/api/v1/sikcasorulansorular/{entity.Id}");
        }
    }
}
