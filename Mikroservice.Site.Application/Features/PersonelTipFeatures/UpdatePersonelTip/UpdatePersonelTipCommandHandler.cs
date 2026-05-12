using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.UpdatePersonelTip
{
    public class UpdatePersonelTipCommandHandler(
          IPersonelTipRepository personelTipRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<UpdatePersonelTipCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdatePersonelTipCommand request, CancellationToken cancellationToken)
        {
            var personelTip = await personelTipRepository.GetByIdAsync(request.Id);
            if (personelTip == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            personelTip.Ad = request.Ad;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            await redisCache.RemoveAsync("personelTip:list", cancellationToken);

            return ServiceResult.Success();
        }
    }
}
