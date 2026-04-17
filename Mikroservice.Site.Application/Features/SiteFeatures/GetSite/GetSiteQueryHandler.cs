using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetSite
{
    public class GetSiteQueryHandler : IRequestHandler<GetSiteQuery, ServiceResult>
    {
        private readonly ISiteRepository _siteRepository;

        public GetSiteQueryHandler(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public async Task<ServiceResult> Handle(GetSiteQuery request, CancellationToken cancellationToken)
        {
            var entity = await _siteRepository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            return ServiceResult.SuccessAsOK(entity);
        }
    }
}
