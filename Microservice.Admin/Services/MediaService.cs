using Microservice.Admin.Services.Contracts;
using Microservice.Admin.Settings;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Microservice.Admin.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMinioClient _minio;
        private readonly MinioSetting _settings;

        public MediaService(IMinioClient minio, IOptions<MinioSetting> settings)
        {
            _minio = minio;
            _settings = settings.Value;
        }

        public async Task<string> UploadAsync(IFormFile file, int siteId, string module)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var objectName = $"site/{siteId}/{module}/{fileName}";

            using var stream = file.OpenReadStream();

            await _minio.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType));

            // 🔥 DOĞRU YAKLAŞIM
            var url = await _minio.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(objectName)
                .WithExpiry(60 * 60));

            return url;
        }
    }
}
