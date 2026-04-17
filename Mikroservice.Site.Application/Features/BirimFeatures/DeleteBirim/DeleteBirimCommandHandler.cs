using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BirimEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mikroservice.Site.Application.Features.BirimFeatures.DeleteBirim
{
    public class DeleteBirimCommandHandler(
          IBirimRepository birimRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
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

            await publishEndpoint.Publish(new BirimDeletedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
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
