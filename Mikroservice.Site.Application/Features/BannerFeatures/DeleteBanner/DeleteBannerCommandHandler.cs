using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BannerFeatures.DeleteBanner
{
    public class DeleteBannerCommandHandler(
          IBannerRepository bannerRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeleteBannerCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
        {
            var banner = await bannerRepository.GetByIdAsync(request.Id);
            if (banner == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            banner.IsDeleted = true;
            bannerRepository.Update(banner);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new BannerDeletedEvent(banner.SiteId, banner.DilId), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
