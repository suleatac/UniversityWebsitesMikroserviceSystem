using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteFeatures.UpdateSite
{
    public class UpdateSiteCommandHandler : IRequestHandler<UpdateSiteCommand, ServiceResult>
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSiteCommandHandler(ISiteRepository siteRepository, IUnitOfWork unitOfWork)
        {
            _siteRepository = siteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(UpdateSiteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _siteRepository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            entity.SiteAdi = request.SiteAdi;
            entity.SiteAdiEng = request.SiteAdiEng;
            entity.SiteUrl = request.SiteUrl;
            entity.BirimId = request.BirimId;
            entity.SiteAlanAdi = request.SiteAlanAdi;
            entity.SiteEPostaSifre = request.SiteEPostaSifre;
            entity.SiteEPostaHost = request.SiteEPostaHost;
            entity.SiteEPostaPort = request.SiteEPostaPort;
            entity.SertifikaParmakIziId = request.SertifikaParmakIziId;
            entity.TemplateId = request.TemplateId;
            entity.IsDeleted = request.IsDeleted;
            entity.SiteEPosta = request.SiteEPosta;
            _siteRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
