using System.Net;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.ReorderPageSections
{
    public sealed class ReorderPageSectionsCommandHandler(
        IPageSectionRepository pageSectionRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<ReorderPageSectionsCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(
            ReorderPageSectionsCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Items == null || request.Items.Count == 0)
                return ServiceResult.Error("Geçersiz istek", "Sıralama için öğe listesi boş.", HttpStatusCode.BadRequest);

            foreach (var item in request.Items)
            {
                var pageSection = await pageSectionRepository.GetByIdAsync(item.Id);
                if (pageSection == null) continue;

                pageSection.Sira = item.Sira;
                pageSectionRepository.Update(pageSection);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache temizleme
            await redisCache.RemoveByPatternAsync("page-sections:list:*", cancellationToken);

            return ServiceResult.Success();
        }
    }
}
