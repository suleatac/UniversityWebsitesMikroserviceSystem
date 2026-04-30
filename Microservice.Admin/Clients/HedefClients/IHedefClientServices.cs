using Microservice.Admin.ViewModels.Hedef;
using Refit;

namespace Microservice.Admin.Clients.HedefClients
{
    public interface IHedefClientServices
    {
        [Get("/api/v1/hedefs")]
        Task<ApiResponse<List<GetHedefVm>>> GetHedefsAsync();
    }
}
