using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypeByTemplateIdAndPageTypeKind;
using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Api.Endpoints.PageTypeEndPoints.EndPoints
{
    public static class GetPageTypeByTemplateIdAndPageTypeKindEndPoint
    {
        public static RouteGroupBuilder GetPageTypeByTemplateIdAndPageTypeKindEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/by-kind/{templateId:int}/{dilId:int}/{pageTypeKind:int}", async (
                IMediator mediator,
                int templateId,
                int dilId,
                int pageTypeKind) =>
                (await mediator.Send(new GetPageTypeByTemplateIdAndPageTypeKindQuery(
                    templateId,
                    dilId,
                    (PageTypeKind)pageTypeKind))).ToGenericResult())
                .WithName("GetPageTypeByTemplateIdAndPageTypeKind")
                .MapToApiVersion(1.0)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
