using Mikroservice.Ogrenci.Application.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Ogrenci.Application.Contracts.Services
{
    public interface IOgrenciExternalApiService
    {
        Task<ApiResponseDto> GetOgrencisAsync(OgrenciSyncRequest request, CancellationToken cancellationToken = default);

    }
}
