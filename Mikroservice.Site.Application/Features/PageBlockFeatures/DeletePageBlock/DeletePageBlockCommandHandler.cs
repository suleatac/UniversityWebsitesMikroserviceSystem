using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.PageBlockFeatures.DeletePageBlock
{
    public class DeletePageBlockCommandHandler(
        IPageBlockRepository pageBlockRepository,
        IPageSectionRepository pageSectionRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<DeletePageBlockCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(
            DeletePageBlockCommand request,
            CancellationToken cancellationToken)
        {
            var block = await pageBlockRepository.GetByIdAsync(request.Id);

            if (block is null || block.IsDeleted)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            // Soft delete: block ve alt satirlari (children) IsDeleted = true.
            block.IsDeleted = true;

            var children = pageBlockRepository
                .Where(b => b.ParentId == block.Id)
                .ToList();

            foreach (var child in children)
            {
                child.IsDeleted = true;
                pageBlockRepository.Update(child);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var section = await pageSectionRepository.GetByIdAsync(block.PageSectionId);

            // 🔥 Cache invalidation
            if (section is not null)
            {
                var key = $"page-sections:list:{section.SiteId}:*";
                await redisCache.RemoveByPatternAsync(key, cancellationToken);
            }

            return ServiceResult.Success();
        }
    }
}
