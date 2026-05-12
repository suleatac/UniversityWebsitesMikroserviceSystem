using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.DeleteTemplate
{
    public class DeleteTemplateCommandHandler(
          ITemplateRepository templateRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<DeleteTemplateCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = await templateRepository.GetByIdAsync(request.Id);
            if (template == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            templateRepository.Delete(template);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            await redisCache.RemoveAsync("template:list", cancellationToken);


            return ServiceResult.Success();
        }
    }
}
