using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PageSectionFeatures.ReorderPageSections;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.PageSectionEndPoints.EndPoints
{
    public static class ReorderPageSectionsEndPoint
    {
        public static RouteGroupBuilder ReorderPageSectionsEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/reorder", async (
                [FromBody] ReorderPageSectionsCommand command,
                [FromServices] IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("ReorderPageSections")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
