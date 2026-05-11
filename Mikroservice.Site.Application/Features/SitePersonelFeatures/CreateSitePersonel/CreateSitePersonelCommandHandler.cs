using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.CreateSitePersonel
{
    public class CreateSitePersonelCommandHandler(
       ISitePersonelRepository repository,
       IUnitOfWork unitOfWork,
       IRedisCacheService redisCache
   ) : IRequestHandler<CreateSitePersonelCommand, ServiceResult<CreateSitePersonelResponse>>
    {
        public async Task<ServiceResult<CreateSitePersonelResponse>> Handle(CreateSitePersonelCommand request, CancellationToken cancellationToken)
        {
            var entity = new SitePersonel
            {
                SiteId = request.SiteId,
                PersonelId = request.PersonelId,
                UnvanId = request.UnvanId,
                PersonelTipId = request.PersonelTipId,

                ResimUrl = request.ResimUrl,
                IlgiAlanlari = request.IlgiAlanlari,
                BlogAdress = request.BlogAdress,
                TwitterAdress = request.TwitterAdress,
                FacebookAdress = request.FacebookAdress,
                InstagramAdress = request.InstagramAdress,
                GoogleplusAdress = request.GoogleplusAdress,

                Hakkinda = request.Hakkinda,
                DeneyimVeCalismalari = request.DeneyimVeCalismalari,

                IsDeleted = false
            };

            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var key = $"sitepersonel:list:{request.SiteId}";
            await redisCache.RemoveAsync(key, cancellationToken);

            var response = new CreateSitePersonelResponse(entity.Id);
            return ServiceResult<CreateSitePersonelResponse>
            .SuccessAsCreated(response, $"/api/v1/sitepersonels/{entity.Id}");
        }
    }
}
