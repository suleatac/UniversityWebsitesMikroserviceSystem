using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.UnvanEvents;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.CreateUnvan
{
    public class CreateUnvanCommandHandler(
     IUnvanRepository repository,
     IUnitOfWork unitOfWork,
     IRedisCacheService redisCache
 ) : IRequestHandler<CreateUnvanCommand, ServiceResult<CreateUnvanResponse>>
    {
        public async Task<ServiceResult<CreateUnvanResponse>> Handle(CreateUnvanCommand request, CancellationToken cancellationToken)
        {
            var entity = new Unvan
            {
                TipId = request.TipId,
                Ad = request.Ad,
                KisaAd = request.KisaAd,
                Sira = request.Sira,
                ParentId = request.ParentId,
            };

            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache invalidation
            await redisCache.RemoveAsync(
                "unvan:list",
                cancellationToken);

            var response = new CreateUnvanResponse(entity.Id);
            return ServiceResult<CreateUnvanResponse>
            .SuccessAsCreated(response, $"/api/v1/unvans/{entity.Id}");
        }
    }
}
