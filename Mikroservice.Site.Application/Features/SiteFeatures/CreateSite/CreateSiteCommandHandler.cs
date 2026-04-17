using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteFeatures.CreateSite
{
    public class CreateSiteCommandHandler : IRequestHandler<CreateSiteCommand, ServiceResult>
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSiteCommandHandler(ISiteRepository siteRepository, IUnitOfWork unitOfWork)
        {
            _siteRepository = siteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(CreateSiteCommand request, CancellationToken cancellationToken)
        {
            var entity = new Site
            {
                SiteAdi = request.SiteAdi,
                SiteAdiEng = request.SiteAdiEng,
                SiteUrl = request.SiteUrl,
                BirimId = request.BirimId,
                SiteAlanAdi = request.SiteAlanAdi,
                SiteEPostaSifre = request.SiteEPostaSifre,
                SiteEPostaHost = request.SiteEPostaHost,
                SiteEPostaPort = request.SiteEPostaPort,
                SertifikaParmakIziId = request.SertifikaParmakIziId,
                TemplateId = request.TemplateId,
                IsDeleted = request.IsDeleted,
                SiteEPosta = request.SiteEPosta
            };
            await _siteRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
