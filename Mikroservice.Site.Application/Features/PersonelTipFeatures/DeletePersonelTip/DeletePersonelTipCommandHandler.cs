using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.DeletePersonelTip
{
    public class DeletePersonelTipCommandHandler(
          IPersonelTipRepository personelTipRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<DeletePersonelTipCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeletePersonelTipCommand request, CancellationToken cancellationToken)
        {
            var personelTip = await personelTipRepository.GetByIdAsync(request.Id);
            if (personelTip == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            personelTipRepository.Delete(personelTip);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            await redisCache.RemoveAsync("personelTip:list", cancellationToken);


            return ServiceResult.Success();
        }
    }
}
