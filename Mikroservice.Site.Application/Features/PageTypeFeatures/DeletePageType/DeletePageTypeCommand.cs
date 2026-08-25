using Microservice.Shared;

namespace Microservice.Site.Application.Features.PageTypeFeatures.DeletePageType
{
    public record DeletePageTypeCommand(int Id) : IRequestByServiceResult;
}