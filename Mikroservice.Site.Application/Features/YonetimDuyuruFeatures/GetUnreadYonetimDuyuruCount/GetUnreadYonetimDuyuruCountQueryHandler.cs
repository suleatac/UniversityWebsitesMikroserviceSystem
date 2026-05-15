using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.GetUnreadYonetimDuyuruCount
{
    public class GetUnreadYonetimDuyuruCountQueryHandler(
        IYonetimDuyuruOkunduRepository okunduRepository
        ) : IRequestHandler<GetUnreadYonetimDuyuruCountQuery, ServiceResult<int>>
    {
        public async Task<ServiceResult<int>> Handle(GetUnreadYonetimDuyuruCountQuery request, CancellationToken cancellationToken)
        {
            var count = await okunduRepository.GetUnreadCountForUserAsync(request.KeycloakUserId, cancellationToken);
            return ServiceResult<int>.SuccessAsOK(count);
        }
    }
}
