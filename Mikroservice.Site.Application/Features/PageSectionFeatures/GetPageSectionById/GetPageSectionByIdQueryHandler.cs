using System.Net;
using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.PageSectionDtos;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.GetPageSectionById
{
    public class GetPageSectionByIdQueryHandler(
        IPageSectionRepository pageSectionRepository,
        ILogger<GetPageSectionByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetPageSectionByIdQuery, ServiceResult<PageSectionDto>>
    {
        public async Task<ServiceResult<PageSectionDto>> Handle(
            GetPageSectionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await pageSectionRepository.GetPageSectionByIdAsync(request.Id, cancellationToken);

            if (entity is null || entity.IsDeleted)
            {
                logger.LogWarning("PageSection bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<PageSectionDto>.Error("Bölüm bulunamadı", HttpStatusCode.NotFound);
            }

            var dto = mapper.Map<PageSectionDto>(entity);
            return ServiceResult<PageSectionDto>.SuccessAsOK(dto);
        }
    }
}
