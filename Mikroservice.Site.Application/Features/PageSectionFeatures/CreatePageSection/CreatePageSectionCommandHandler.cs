using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.CreatePageSection
{
    public class CreatePageSectionCommandHandler(
        IPageSectionRepository pageSectionRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<CreatePageSectionCommand, ServiceResult<CreatePageSectionResponse>>
    {
        public async Task<ServiceResult<CreatePageSectionResponse>> Handle(
            CreatePageSectionCommand request,
            CancellationToken cancellationToken)
        {
            // Sira verilmemis ise listenin sonuna ekle.
            if (request.Sira <= 0)
            {
                var maxSira = pageSectionRepository
                    .GetAll()
                    .Where(s => s.SiteId == request.SiteId && s.DilId == request.DilId)
                    .Select(s => (int?)s.Sira)
                    .Max() ?? 0;

                request.Sira = maxSira + 1;
            }

            var pageSection = new PageSection {
                SiteId = request.SiteId,
                DilId = request.DilId,
                Baslik = request.Baslik,
                BackgroundColor = request.BackgroundColor,
                BackgroundImageUrl = request.BackgroundImageUrl,
                Sira = request.Sira,
                Yayinda = request.Yayinda,
                OlusturulmaTarihi = DateTime.Now,
                IsDeleted = false
            };

            await pageSectionRepository.AddAsync(pageSection);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 cache temizleme islemi
            var key = $"page-sections:list:{pageSection.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            var response = new CreatePageSectionResponse(pageSection.Id);
            return ServiceResult<CreatePageSectionResponse>
                .SuccessAsCreated(response, $"/api/v1/page-sections/{pageSection.Id}");
        }
    }
}
