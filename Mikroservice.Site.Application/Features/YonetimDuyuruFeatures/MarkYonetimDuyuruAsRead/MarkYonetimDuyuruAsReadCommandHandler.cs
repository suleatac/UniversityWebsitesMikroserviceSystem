using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.MarkYonetimDuyuruAsRead
{
    public class MarkYonetimDuyuruAsReadCommandHandler(
        IYonetimDuyuruOkunduRepository okunduRepository,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<MarkYonetimDuyuruAsReadCommand, ServiceResult<bool>>
    {
        public async Task<ServiceResult<bool>> Handle(MarkYonetimDuyuruAsReadCommand request, CancellationToken cancellationToken)
        {
            // Daha önce okunmuş mu kontrol et
            var existing = await okunduRepository.GetByDuyuruIdAndUserIdAsync(request.Id, request.KeycloakUserId, cancellationToken);
            if (existing is not null)
                return ServiceResult<bool>.SuccessAsOK(true); // Zaten okunmuş

            var okundu = new YonetimDuyuruOkundu
            {
                YonetimDuyuruId = request.Id,
                KeycloakUserId = request.KeycloakUserId,
                OkunmaTarihi = DateTime.Now
            };

            await okunduRepository.AddAsync(okundu);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult<bool>.SuccessAsOK(true);
        }
    }
}
