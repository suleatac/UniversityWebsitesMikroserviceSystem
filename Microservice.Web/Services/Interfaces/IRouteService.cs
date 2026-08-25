using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.Interfaces
{
    public interface IRouteService
    {
        Task<RouteResolveResult?> ResolveAsync(
           string host,
           string path);
    }
}
