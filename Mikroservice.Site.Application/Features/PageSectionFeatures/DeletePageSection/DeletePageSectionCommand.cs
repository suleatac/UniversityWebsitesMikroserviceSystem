using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.DeletePageSection
{
    public record DeletePageSectionCommand(int Id) : IRequestByServiceResult;
}
