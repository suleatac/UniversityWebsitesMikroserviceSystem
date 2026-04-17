using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.CreateSiteOzellikleri
{
    public class CreateSiteOzellikleriCommandHandler : IRequestHandler<CreateSiteOzellikleriCommand, ServiceResult>
    {
        private readonly ISiteOzellikleriRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSiteOzellikleriCommandHandler(ISiteOzellikleriRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(CreateSiteOzellikleriCommand request, CancellationToken cancellationToken)
        {
            var entity = new SiteOzellikleri
            {
                SiteId = request.SiteId,
                SiteAdress = request.SiteAdress,
                SiteAdressEng = request.SiteAdressEng,
                SiteBaslangicHakkimizda = request.SiteBaslangicHakkimizda,
                SiteBaslangicHakkimizdaEng = request.SiteBaslangicHakkimizdaEng,
                SiteTelNo = request.SiteTelNo,
                SiteFaxNo = request.SiteFaxNo,
                SiteFacebookAdress = request.SiteFacebookAdress,
                SiteTwitterAdress = request.SiteTwitterAdress,
                SiteInstagramAdress = request.SiteInstagramAdress,
                SiteYoutubeAdress = request.SiteYoutubeAdress,
                SiteHaritaAdress = request.SiteHaritaAdress,
                SiteBaslangicVideoLink = request.SiteBaslangicVideoLink,
                SiteBaslangicVideoResimAdress = request.SiteBaslangicVideoResimAdress,
                SiteWatsappAdress = request.SiteWatsappAdress,
                SiteLinkedinAdress = request.SiteLinkedinAdress,
                SiteHakkindaLink = request.SiteHakkindaLink,
                SiteVideoType = request.SiteVideoType,
                SiteHakkindaResim = request.SiteHakkindaResim,
                SiteFooterLogo = request.SiteFooterLogo,
                SiteTopbarLogo = request.SiteTopbarLogo
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
