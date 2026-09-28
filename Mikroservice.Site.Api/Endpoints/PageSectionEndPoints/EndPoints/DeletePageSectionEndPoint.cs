using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PageSectionFeatures.DeletePageSection;

namespace Mikroservice.Site.Api.Endpoints.PageSectionEndPoints.EndPoints
{
    public static class DeletePageSectionEndPoint
    {
        public static RouteGroupBuilder DeletePageSectionEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (
                int id,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new DeletePageSectionCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeletePageSection")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
