using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PageBlockFeatures.DeletePageBlock;

namespace Mikroservice.Site.Api.Endpoints.PageSectionEndPoints.EndPoints
{
    public static class DeletePageBlockEndPoint
    {
        public static RouteGroupBuilder DeletePageBlockEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/blocks/{blockId:int}", async (
                int blockId,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new DeletePageBlockCommand(blockId));
                return result.ToGenericResult();
            })
            .WithName("DeletePageBlock")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
