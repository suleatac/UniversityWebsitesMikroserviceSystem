using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.UpdateTemplate
{
    public class UpdateTemplateCommandHandler(
          ITemplateRepository templateRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<UpdateTemplateCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = await templateRepository.GetByIdAsync(request.Id);
            if (template == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            template.TemplateAdi = request.TemplateAdi;
            template.TemplateTuru = request.TemplateTuru;
            template.FolderName = request.FolderName;
            template.LayoutPath = request.LayoutPath;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            await redisCache.RemoveAsync("template:list", cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
