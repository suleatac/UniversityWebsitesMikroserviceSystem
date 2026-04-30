using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IHedefService
    {
        Task<ServiceResult<List<GetHedefVm>>> GetHedefsAsync();
    }
}
