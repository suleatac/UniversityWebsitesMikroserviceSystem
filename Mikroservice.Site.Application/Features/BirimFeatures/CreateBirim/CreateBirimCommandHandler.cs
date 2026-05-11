using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BirimFeatures.CreateBirim
{
    public class CreateBirimCommandHandler(
     IBirimRepository birimRepository,
     IUnitOfWork unitOfWork,
     IRedisCacheService redisCache
 ) : IRequestHandler<CreateBirimCommand, ServiceResult<CreateBirimResponse>>
    {
        public async Task<ServiceResult<CreateBirimResponse>> Handle(CreateBirimCommand request, CancellationToken cancellationToken)
        {
            var birim = new Domain.Entities.Birim {
                Ad = request.Ad,
                ParentId = request.ParentId,
                Sira = request.Sira
               
            };

            await birimRepository.AddAsync(birim);
            await unitOfWork.SaveChangesAsync(cancellationToken);


            await redisCache.RemoveAsync("birim:list", cancellationToken);

            var response = new CreateBirimResponse(birim.Id);
            return ServiceResult<CreateBirimResponse>
            .SuccessAsCreated(response, $"/api/v1/birims/{birim.Id}");

        }
    }
}
