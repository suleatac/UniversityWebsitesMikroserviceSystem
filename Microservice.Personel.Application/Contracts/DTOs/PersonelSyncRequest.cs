namespace Microservice.Personel.Application.Contracts.DTOs
{
 
        public record PersonelSyncRequest(string ServiceName, object ServiceCriteria);
        public record PersonelSyncResponse(List<Domain.Entities.Personel> Personels, int TotalCount);
        public record ApiResponseDto(bool IsSuccess, string? ErrorMessage, string RawContent);

    
}
