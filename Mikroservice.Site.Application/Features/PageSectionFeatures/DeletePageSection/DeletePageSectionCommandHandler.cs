using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.DeletePageSection
{
    public class DeletePageSectionCommandHandler(
        IPageSectionRepository pageSectionRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<DeletePageSectionCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(
            DeletePageSectionCommand request,
            CancellationToken cancellationToken)
        {
            var pageSection = await pageSectionRepository.GetByIdAsync(request.Id);

            if (pageSection is null || pageSection.IsDeleted)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            pageSection.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache invalidation
            var key = $"page-sections:list:{pageSection.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
