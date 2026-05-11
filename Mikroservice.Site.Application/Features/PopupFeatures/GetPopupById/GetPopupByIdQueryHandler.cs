using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.PopupDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.PopupFeatures.GetPopupById
{
    public class GetPopupByIdQueryHandler(
        IPopupRepository popupRepository,
        ILogger<GetPopupByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetPopupByIdQuery, ServiceResult<PopupDetailDto>>
    {
        public async Task<ServiceResult<PopupDetailDto>> Handle(GetPopupByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await popupRepository.GetByIdAsync(request.Id);

            if (entity is null || entity.IsDeleted)
            {
                logger.LogWarning("Popup bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<PopupDetailDto>.Error("Popup bulunamadı", HttpStatusCode.NotFound);
            }

            var dto = mapper.Map<PopupDetailDto>(entity);

            logger.LogInformation("Popup DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<PopupDetailDto>.SuccessAsOK(dto);
        }
    }
}