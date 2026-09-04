using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.Interfaces
{
    public interface IPageDetailResolver
    {
        bool CanResolve(PageTypeKindEnum pageType);

        Task<RouteResolveResult?> ResolveAsync(
            RouteResolveResult result,
            string detailSlug);
    }
}
