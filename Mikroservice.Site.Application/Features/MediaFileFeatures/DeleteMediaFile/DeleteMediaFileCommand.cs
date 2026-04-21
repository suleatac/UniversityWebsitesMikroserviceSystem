using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.MediaFileFeatures.DeleteMediaFile
{
    public record DeleteMediaFileCommand(int Id) : IRequestByServiceResult;

}
