using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.UpdateSitePersonel
{
    public class UpdateSitePersonelCommandHandler(
       ISitePersonelRepository repository,
       IUnitOfWork unitOfWork,
       IRedisCacheService redisCache
   ) : IRequestHandler<UpdateSitePersonelCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateSitePersonelCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.UnvanId = request.UnvanId;
            entity.PersonelTipId = request.PersonelTipId;

            entity.ResimUrl = request.ResimUrl;
            entity.IlgiAlanlari = request.IlgiAlanlari;

            entity.BlogAdress = request.BlogAdress;
            entity.TwitterAdress = request.TwitterAdress;
            entity.FacebookAdress = request.FacebookAdress;
            entity.InstagramAdress = request.InstagramAdress;
            entity.GoogleplusAdress = request.GoogleplusAdress;

            entity.Hakkinda = request.Hakkinda;
            entity.DeneyimVeCalismalari = request.DeneyimVeCalismalari;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var key = $"sitepersonel:list:{request.SiteId}";
            await redisCache.RemoveAsync(key, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
