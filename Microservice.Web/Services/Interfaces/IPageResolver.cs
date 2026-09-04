using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.Interfaces
{
    public interface IPageResolver
    {
        bool CanResolve(PageTypeKindEnum pageType);

        Task ResolveAsync(RouteResolveResult result);
    }
}