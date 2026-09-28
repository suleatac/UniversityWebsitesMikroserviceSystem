using System.Net;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Features.Common;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PageBlockFeatures.CreatePageBlock
{
    public class CreatePageBlockCommandHandler(
        IPageBlockRepository pageBlockRepository,
        IPageSectionRepository pageSectionRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<CreatePageBlockCommand, ServiceResult<CreatePageBlockResponse>>
    {
        public async Task<ServiceResult<CreatePageBlockResponse>> Handle(
            CreatePageBlockCommand request,
            CancellationToken cancellationToken)
        {
            var section = await pageSectionRepository.GetByIdAsync(request.PageSectionId);
            if (section is null || section.IsDeleted)
            {
                return ServiceResult<CreatePageBlockResponse>.Error(
                    "Bölüm bulunamadı",
                    $"PageSectionId: {request.PageSectionId} için bölüm bulunamadı.",
                    HttpStatusCode.NotFound);
            }

            // RowNumber verilmemis ise satir sonuna ekle.
            if (request.RowNumber <= 0)
            {
                request.RowNumber = pageBlockRepository
                    .Where(b => b.PageSectionId == request.PageSectionId && b.ParentId == request.ParentId)
                    .Select(b => (int?)b.RowNumber)
                    .Max() ?? 0;

                request.RowNumber += 1;
            }

            var block = new PageBlock {
                PageSectionId = request.PageSectionId,
                ParentId = request.ParentId,
                ContentType = request.ContentType,
                Content = request.Content,
                VideoUrl = request.VideoUrl,
                VideoType = request.VideoType,
                BackgroundImageUrl = request.BackgroundImageUrl,
                BackgroundColor = request.BackgroundColor,
                ColumnSize = request.ColumnSize,
                RowNumber = request.RowNumber,
                Animation = request.Animation,
                IsDeleted = false
            };

            // Carousel slaytlari parent navigation kumesine ekle (EF cascade ile kaydeder).
            PageBlockMediaSync.Apply(block.Medias, request.Medias);

            await pageBlockRepository.AddAsync(block);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 cache temizleme islemi
            var key = $"page-sections:list:{section.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            var response = new CreatePageBlockResponse(block.Id);
            return ServiceResult<CreatePageBlockResponse>
                .SuccessAsCreated(response, $"/api/v1/page-blocks/{block.Id}");
        }
    }
}
