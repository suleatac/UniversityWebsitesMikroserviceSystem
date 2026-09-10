using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Mikroservice.Site.Application.Features.IcerikFeatures.IsSeoUrlAvailable
{
    public class IsSeoUrlAvailableQueryHandler(IIcerikRepository icerikRepository)
        : IRequestHandler<IsSeoUrlAvailableQuery, ServiceResult<bool>>
    {
        public async Task<ServiceResult<bool>> Handle(IsSeoUrlAvailableQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SeoUrl))
                return ServiceResult<bool>.Error("SeoUrl bos olamaz", HttpStatusCode.BadRequest);

            // Icerik tablosu (Haber, Duyuru, Bilgi, Etkinlik, Video, Banner - TPH) uzerinde
            // benzersiz index (SiteId, SeoUrl) oldugundan kontrol site genelinde yapilir.
            // Not: GetAll() uzerindeki soft-delete query filter otomatik uygulanir.
            var icerikTaken = await icerikRepository.GetAll()
                .AnyAsync(x => x.SiteId == request.SiteId
                                && x.SeoUrl == request.SeoUrl
                                && x.PageTypeId == request.PageTypeId
                                && (!request.ExcludeIcerikId.HasValue || x.Id != request.ExcludeIcerikId.Value),
                    cancellationToken);

            if (icerikTaken)
                return ServiceResult<bool>.Success(false);

            return ServiceResult<bool>.Success(true);
        }
    }
}
