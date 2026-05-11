using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.CreatePersonelTip
{
    public class CreatePersonelTipCommandHandler(
          IPersonelTipRepository personelTipRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<CreatePersonelTipCommand, ServiceResult<CreatePersonelTipResponse>>
    {
        public async Task<ServiceResult<CreatePersonelTipResponse>> Handle(CreatePersonelTipCommand request, CancellationToken cancellationToken)
        {
            var newPersonelTip = new PersonelTip {
                Ad = request.Ad
            };
            await personelTipRepository.AddAsync(newPersonelTip);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            await redisCache.RemoveAsync("personelTip:list", cancellationToken);


            var response = new CreatePersonelTipResponse(newPersonelTip.Id);
            return ServiceResult<CreatePersonelTipResponse>
            .SuccessAsCreated(response, $"/api/v1/personeltips/{newPersonelTip.Id}");
        }
    }
}
