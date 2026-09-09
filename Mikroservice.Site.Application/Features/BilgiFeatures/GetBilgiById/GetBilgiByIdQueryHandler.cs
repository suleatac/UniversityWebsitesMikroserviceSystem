using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.BilgiDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.GetBilgiById
{
    public class GetBilgiByIdQueryHandler(
    IBilgiRepository bilgiRepository,
    ILogger<GetBilgiByIdQueryHandler> logger,
    IMapper mapper
) : IRequestHandler<GetBilgiByIdQuery, ServiceResult<BilgiDetailDto>>
    {
        public async Task<ServiceResult<BilgiDetailDto>> Handle(
            GetBilgiByIdQuery request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Bilgi getiriliyor. Id: {Id}",
                request.Id);
            var entity = await bilgiRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning(
                    "Bilgi bulunamadı. Id: {Id}",
                    request.Id);

                return ServiceResult<BilgiDetailDto>.Error(
                    "Bilgi bulunamadı",
                    HttpStatusCode.NotFound);
            }

            var dto = mapper.Map<BilgiDetailDto>(entity);

            if (dto is null)
            {
                logger.LogError(
                    "Bilgi DTO'ya dönüştürülemedi. Id: {Id}",
                    request.Id);

                return ServiceResult<BilgiDetailDto>.Error(
                    "Bilgi verisi oluşturulamadı",
                    HttpStatusCode.InternalServerError);
            }

            logger.LogInformation(
                "Bilgi başarıyla getirildi. Id: {Id}",
                request.Id);

            return ServiceResult<BilgiDetailDto>.SuccessAsOK(dto);
        }
    }
}