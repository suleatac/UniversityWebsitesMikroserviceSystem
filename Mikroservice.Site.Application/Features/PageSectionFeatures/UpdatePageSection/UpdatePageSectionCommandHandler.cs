using System.Net;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.UpdatePageSection
{
    public class UpdatePageSectionCommandHandler(
        IPageSectionRepository pageSectionRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<UpdatePageSectionCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(
            UpdatePageSectionCommand request,
            CancellationToken cancellationToken)
        {
            var pageSection = await pageSectionRepository.GetByIdAsync(request.Id);

            if (pageSection is null || pageSection.IsDeleted)
            {
                return ServiceResult.Error("Bölüm bulunamadı", $"Id: {request.Id} için bölüm bulunamadı.", HttpStatusCode.NotFound);
            }

            pageSection.SiteId = request.SiteId;
            pageSection.DilId = request.DilId;
            pageSection.Baslik = request.Baslik;
            pageSection.BackgroundColor = request.BackgroundColor;
            pageSection.BackgroundImageUrl = request.BackgroundImageUrl;
            pageSection.Sira = request.Sira;
            pageSection.Yayinda = request.Yayinda;

            pageSectionRepository.Update(pageSection);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache invalidation
            var key = $"page-sections:list:{pageSection.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
