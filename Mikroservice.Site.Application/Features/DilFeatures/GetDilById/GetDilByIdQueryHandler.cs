using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;
using System.Net;

namespace Mikroservice.Site.Application.Features.DilFeatures.GetDilById
{
    public class GetDilByIdQueryHandler(
        IDilRepository dilRepository,
        ILogger<GetDilByIdQueryHandler> logger)
        : IRequestHandler<GetDilByIdQuery, ServiceResult<Dil>>
    {
        public async Task<ServiceResult<Dil>> Handle(GetDilByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await dilRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("Dil bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<Dil>.Error("Dil bulunamadı", HttpStatusCode.NotFound);
            }

            logger.LogInformation("Dil DB'den alındı. Id: {Id}", request.Id);
            return ServiceResult<Dil>.SuccessAsOK(entity);
        }
    }
}
