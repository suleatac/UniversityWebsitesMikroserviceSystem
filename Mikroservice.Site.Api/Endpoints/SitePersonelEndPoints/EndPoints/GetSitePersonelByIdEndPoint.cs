using MediatR;
using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonelById;

namespace Mikroservice.Site.Api.Endpoints.SitePersonelEndPoints.EndPoints
{
    public static class GetSitePersonelByIdEndPoint
    {
        public static RouteGroupBuilder GetSitePersonelByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetSitePersonelByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetSitePersonelById")
            .MapToApiVersion(1.0)
            .Produces<SitePersonel>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}