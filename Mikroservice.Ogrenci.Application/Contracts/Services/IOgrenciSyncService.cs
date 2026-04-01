using Mikroservice.Ogrenci.Application.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Ogrenci.Application.Contracts.Services
{
    public interface IOgrenciSyncService
    {
        Task<OgrenciSyncResponse> SyncOgrencisAsync(DateTime? lastUpdateDate = null, CancellationToken cancellationToken = default);
    }
}
