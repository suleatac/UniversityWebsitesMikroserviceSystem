
using Microservice.Personel.Application.Contracts.DTOs;

namespace Mikroservice.Personel.Application.Contracts.Services
{
    public interface IPersonelExternalApiService
    {
        Task<ApiResponseDto> GetPersonelsAsync(PersonelSyncRequest request, CancellationToken cancellationToken = default);

    }
}
