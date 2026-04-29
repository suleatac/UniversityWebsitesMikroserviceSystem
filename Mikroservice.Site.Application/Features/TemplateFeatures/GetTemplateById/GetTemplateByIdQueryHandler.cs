using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.TemplateDtos;
using Mikroservice.Site.Application.Features.TemplateFeatures.GetTemplateById;
using System.Net;

public class GetTemplateByIdQueryHandler(
    ITemplateRepository templateRepository,
    ILogger<GetTemplateByIdQueryHandler> logger,
    IMapper mapper
) : IRequestHandler<GetTemplateByIdQuery, ServiceResult<TemplateDetailDto>>
{
    public async Task<ServiceResult<TemplateDetailDto>> Handle(GetTemplateByIdQuery request, CancellationToken cancellationToken)
    {

        // ✔ DB'den TEK kayıt çek
        var entity = await templateRepository.GetByIdAsync(request.Id);

        if (entity is null)
        {
            logger.LogWarning("Template bulunamadı. Id: {Id}", request.Id);

            return ServiceResult<TemplateDetailDto>.Error("Template bulunamadı", HttpStatusCode.NotFound);
        }

        // ✔ map
        var dto = mapper.Map<TemplateDetailDto>(entity);

        logger.LogInformation("Template DB'den alındı. Id: {Id}", request.Id);

        return ServiceResult<TemplateDetailDto>.SuccessAsOK(dto);
    }
}