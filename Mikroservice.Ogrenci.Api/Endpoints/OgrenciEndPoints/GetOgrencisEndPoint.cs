using MediatR;
using Microservice.Ogrenci.Application.Features.OgrenciFeatures.GetOgrenci;
using Microservice.Shared.Extentions;

namespace Mikroservice.Ogrenci.Api.Endpoints.OgrenciEndPoints
{
    public static class GetOgrencisEndPoint
    {
        public static RouteGroupBuilder GetOgrencisGroupItemEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) => {
                var result = await mediator.Send(new GetOgrencisQuery());
                return result.ToGenericResult();
            })
             .WithName("GetOgrencis")
             .MapToApiVersion(1.0)
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
