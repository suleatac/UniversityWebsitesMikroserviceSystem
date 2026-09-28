using System.Net;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Features.Common;

namespace Mikroservice.Site.Application.Features.PageBlockFeatures.UpdatePageBlock
{
    public class UpdatePageBlockCommandHandler(
        IPageBlockRepository pageBlockRepository,
        IPageSectionRepository pageSectionRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<UpdatePageBlockCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(
            UpdatePageBlockCommand request,
            CancellationToken cancellationToken)
        {
            // Medyalariyla birlikte TRACKED yukle (collection senkronizasyonu izlenebilsin).
            var block = await pageBlockRepository.GetTrackedWithMediasAsync(request.Id, cancellationToken);
            if (block is null || block.IsDeleted)
            {
                return ServiceResult.Error("Container bulunamadı", $"Id: {request.Id} için block bulunamadı.", HttpStatusCode.NotFound);
            }

            var section = await pageSectionRepository.GetByIdAsync(request.PageSectionId);
            if (section is null || section.IsDeleted)
            {
                return ServiceResult.Error("Bölüm bulunamadı", $"PageSectionId: {request.PageSectionId} için bölüm bulunamadı.", HttpStatusCode.NotFound);
            }

            // Nested satir bolumunde parent kendini isaret etmemeli.
            if (request.ParentId == block.Id)
            {
                return ServiceResult.Error("Geçersiz istek", "Block kendi kendine parent olamaz.", HttpStatusCode.BadRequest);
            }

            block.PageSectionId = request.PageSectionId;
            block.ParentId = request.ParentId;
            block.ContentType = request.ContentType;
            block.Content = request.Content;
            block.VideoUrl = request.VideoUrl;
            block.VideoType = request.VideoType;
            block.BackgroundImageUrl = request.BackgroundImageUrl;
            block.BackgroundColor = request.BackgroundColor;
            block.ColumnSize = request.ColumnSize;
            block.RowNumber = request.RowNumber;
            block.Animation = request.Animation;

            // Carousel slaytlarini senkronize et.
            PageBlockMediaSync.Apply(block.Medias, request.Medias);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache invalidation
            var key = $"page-sections:list:{section.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
