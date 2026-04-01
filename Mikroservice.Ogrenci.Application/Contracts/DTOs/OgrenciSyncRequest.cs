namespace Mikroservice.Ogrenci.Application.Contracts.DTOs
{
 
        public record OgrenciSyncRequest(string ServiceName, object ServiceCriteria);
        public record OgrenciSyncResponse(List<Microservice.Ogrenci.Domain.Entities.Ogrenci> Students, int TotalCount);
        public record ApiResponseDto(bool IsSuccess, string? ErrorMessage, string RawContent);

    
}
