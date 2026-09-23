using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using System.Net;

namespace Mikroservice.Site.Application.Features.IcerikFeatures.IsSeoUrlAvailable
{
    public class IsSeoUrlAvailableQueryHandler(
        IIcerikRepository icerikRepository,
        ISitePersonelRepository sitePersonelRepository
        )
        : IRequestHandler<IsSeoUrlAvailableQuery, ServiceResult<bool>>
    {
        public async Task<ServiceResult<bool>> Handle(IsSeoUrlAvailableQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SeoUrl))
                return ServiceResult<bool>.Error("SeoUrl bos olamaz", HttpStatusCode.BadRequest);

            // Icerik tablosu (Haber, Duyuru, Bilgi, Etkinlik, Video, Banner - TPH) uzerinde
            // benzersiz index (SiteId, SeoUrl) oldugundan kontrol site genelinde ve TUM
            // sayfa tipleri icin yapilir. PageTypeId filtresi index ile eslesmiyordu:
            // slug'i baska bir icerik tipi almussa kontrol "bos" diyor, DB 23505 patlatiyordu.
            // Ayni sekilde ExcludeIcerikId yalnizca Icerik sorgusuna verilir; SitePersonel
            // ayri bir tablo/primary-key alani oldugundan oraya ExcludeSitePersonelId gecerlidir.
            // Not: GetAll() uzerindeki soft-delete query filter otomatik uygulanir.
            var isSeoUrlTakenByIcerik = await icerikRepository
                .IsSeoUrlTakenAsync(request.SiteId, request.SeoUrl, request.ExcludeIcerikId, cancellationToken);
            var isSeoUrlTakenBySitePersonel = await sitePersonelRepository
                .IsSeoUrlTakenAsync(request.SiteId, request.SeoUrl, request.ExcludeSitePersonelId, cancellationToken);



            if (isSeoUrlTakenByIcerik || isSeoUrlTakenBySitePersonel)
                return ServiceResult<bool>.Success(false);

            return ServiceResult<bool>.Success(true);
        }
    }
}
