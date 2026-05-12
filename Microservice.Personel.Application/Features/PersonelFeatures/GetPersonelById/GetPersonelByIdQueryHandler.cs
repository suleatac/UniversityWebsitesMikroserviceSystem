using MediatR;
using Microservice.Personel.Application.Contracts.IRepositories;
using Microservice.Shared;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Microservice.Personel.Application.Features.PersonelFeatures.GetPersonelById
{
    public class GetPersonelByIdQueryHandler(
        IPersonelRepository personelRepository,
        ILogger<GetPersonelByIdQueryHandler> logger
    ) : IRequestHandler<GetPersonelByIdQuery, ServiceResult<Domain.Entities.Personel>>
    {
        public async Task<ServiceResult<Domain.Entities.Personel>> Handle(GetPersonelByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await personelRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("Bilgi bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<Domain.Entities.Personel>.Error("Bilgi bulunamadı", HttpStatusCode.NotFound);
            }

            logger.LogInformation("Bilgi DB'den alındı. Id: {Id}", entity);

            return ServiceResult<Domain.Entities.Personel>.SuccessAsOK(entity);
        }
    }
}
