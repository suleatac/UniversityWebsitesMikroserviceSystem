using Microservice.Admin.Clients.TemplateClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Template;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateClientService _templateClientService;
        private readonly ILogger<TemplateService> _logger;

        public TemplateService(ITemplateClientService templateClientService, ILogger<TemplateService> logger)
        {
            _templateClientService = templateClientService ?? throw new ArgumentNullException(nameof(templateClientService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // 🔹 GET ALL
        public async Task<ServiceResult<List<GetTemplateVm>>> GetTemplatesAsync()
        {
            _logger.LogInformation("Template listesi getiriliyor.");

            var response = await _templateClientService.GetTemplatesAsync();

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                   ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                   : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );


                return ServiceResult<List<GetTemplateVm>>.Error(
                problemDetails?.Detail ?? problemDetails?.Title ?? "Template listesi alınamadı.");
            }

            _logger.LogInformation("Template listesi başarıyla alındı. Count: {Count}", response.Content?.Count);

            return ServiceResult<List<GetTemplateVm>>.Success(response.Content!);
        }

       

        // 🔹 CREATE
        public async Task<ServiceResult<bool>> CreateTemplateAsync(CreateTemplateVm dto)
        {
            //_logger.LogInformation("Yeni template oluşturuluyor. Ad: {Name}", dto.TemplateAdi);

            var response = await _templateClientService.CreateTemplateAsync(dto);

            //if (!response.IsSuccessStatusCode)
            //{
            //    var problemDetails = response.Error != null
            //      ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
            //      : null;

            //    _logger.LogError(
            //        "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
            //        response.StatusCode,
            //        problemDetails?.Title,
            //        problemDetails?.Detail
            //    );


            //    return ServiceResult<bool>.Error(
            //    problemDetails?.Detail ?? problemDetails?.Title ?? "Template oluşturulamadı.");


            //}

            _logger.LogInformation("Template oluşturuldu. Id: {Id}", response);

            return ServiceResult<bool>.Success(true);
        }

       
    }
}
