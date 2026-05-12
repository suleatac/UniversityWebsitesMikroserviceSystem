using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mikroservice.Site.Application.Features.BirimFeatures.DeleteBirim
{
    public class DeleteBirimCommandHandler(
          IBirimRepository birimRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<DeleteBirimCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteBirimCommand request, CancellationToken cancellationToken)
        {
            var birim = await birimRepository.GetByIdAsync(request.Id);

            if (birim == null || birim.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            await SoftDeleteTree(request.Id);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await redisCache.RemoveAsync("birim:list", cancellationToken);

            return ServiceResult.Success();
        }
        private async Task SoftDeleteTree(int parentId)
        {
            var children = await birimRepository.Where(x => x.ParentId == parentId).ToListAsync();

            foreach (var child in children)
            {
                await SoftDeleteTree(child.Id);
            }

            var entity = await birimRepository.GetByIdAsync(parentId);
            if (entity != null)
            {
                entity.IsDeleted = true;
            }
        }
    }

   
    }
