using Microservice.Web.Clients.VideoClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Video;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class VideoService :IVideoService
    {
        private readonly IVideoClientServices _videoClient;
        private readonly ILogger<VideoService> _logger;

        public VideoService(IVideoClientServices videoClient, ILogger<VideoService> logger)
        {
            _videoClient = videoClient ?? throw new ArgumentNullException(nameof(videoClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        public async Task<ServiceResult<VideoDetailVm>> GetVideoByIdAsync(int id)
        {
            _logger.LogInformation("Video getiriliyor. Id: {Id}", id);
            var response = await _videoClient.GetVideoByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<VideoDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Video bulunamadı");
            }

            return ServiceResult<VideoDetailVm>.Success(response.Content!);
        }
    }
}
