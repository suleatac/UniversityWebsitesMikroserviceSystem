using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BirimFeatures.UpdateBirim
{
    public class UpdateBirimCommandHandler(
          IBirimRepository birimRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<UpdateBirimCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateBirimCommand request, CancellationToken cancellationToken)
        {
            var birim = await birimRepository.GetByIdAsync(request.Id);
            if (birim == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            birim.Ad = request.Ad;
            birim.ParentId = request.ParentId;
            birim.Sira = request.Sira;

       
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            await redisCache.RemoveAsync("birim:list", cancellationToken);


            return ServiceResult.Success();
        }
    }
}
