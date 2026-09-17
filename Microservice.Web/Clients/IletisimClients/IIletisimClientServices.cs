using Microservice.Web.ViewModels.Iletisim;
using Refit;

namespace Microservice.Web.Clients.IletisimClients
{
    public interface IIletisimClientServices
    {
        [Post("/api/v1/iletisim-mesajlari")]
        Task<ApiResponse<object>> GonderAsync([Body] IletisimMesajiVm dto);
    }
}
