using Microservice.Admin.Clients.PageTypeClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.PageType;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class PageTypeService(IPageTypeClientService client, ILogger<PageTypeService> logger) : IPageTypeService
    {
        public async Task<ServiceResult<List<GetPageTypeVm>>> GetPageTypesAsync()
        {
            var response = await client.GetPageTypesAsync();
            if (!response.IsSuccessStatusCode)
                return ServiceResult<List<GetPageTypeVm>>.Error(GetError(response.Error?.Content, "PageType listesi alınamadı."));
            return ServiceResult<List<GetPageTypeVm>>.Success(response.Content ?? []);
        }

        public async Task<ServiceResult<GetPageTypeVm>> GetPageTypeByIdAsync(int id)
        {
            var response = await client.GetPageTypeByIdAsync(id);
            if (!response.IsSuccessStatusCode)
                return ServiceResult<GetPageTypeVm>.Error(GetError(response.Error?.Content, "PageType bulunamadı."));
            return ServiceResult<GetPageTypeVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<bool>> CreatePageTypeAsync(CreatePageTypeVm model)
        {
            var response = await client.CreatePageTypeAsync(model);
            return response.IsSuccessStatusCode
                ? ServiceResult<bool>.Success(true)
                : ServiceResult<bool>.Error(GetError(response.Error?.Content, "PageType oluşturulamadı."));
        }

        public async Task<ServiceResult<bool>> UpdatePageTypeAsync(UpdatePageTypeVm model)
        {
            var response = await client.UpdatePageTypeAsync(model.Id, model);
            return response.IsSuccessStatusCode
                ? ServiceResult<bool>.Success(true)
                : ServiceResult<bool>.Error(GetError(response.Error?.Content, "PageType güncellenemedi."));
        }

        public async Task<ServiceResult<bool>> DeletePageTypeAsync(int id)
        {
            var response = await client.DeletePageTypeAsync(id);
            return response.IsSuccessStatusCode
                ? ServiceResult<bool>.Success(true)
                : ServiceResult<bool>.Error(GetError(response.Error?.Content, "PageType silinemedi."));
        }

        private static string GetError(string? content, string fallback)
        {
            if (string.IsNullOrWhiteSpace(content)) return fallback;
            try
            {
                var problem = JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(content);
                return problem?.Detail ?? problem?.Title ?? fallback;
            }
            catch (JsonException) { return fallback; }
        }
    }
}