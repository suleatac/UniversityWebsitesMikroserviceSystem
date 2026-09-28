using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.PageBlockFeatures.DeletePageBlock
{
    public record DeletePageBlockCommand(int Id) : IRequestByServiceResult;
}
