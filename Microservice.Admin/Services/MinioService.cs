using Microservice.Admin.Services.Interfaces;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using System.Reactive.Linq;

namespace Microservice.Admin.Services
{
    public class MinioService: IMinioService
    {
        private readonly IMinioClient _minio;
        private const string BucketName = "files";

        public MinioService()
        {
            _minio = new MinioClient()
                .WithEndpoint("localhost:9000")
                .WithCredentials("minioadmin", "minioadmin")
                .Build();
        }

        public async Task<List<string>> GetFilesAsync(string prefix)
        {
            var files = new List<string>();

            var args = new ListObjectsArgs()
                .WithBucket(BucketName)
                .WithPrefix(prefix)
                .WithRecursive(true);

            var observable = _minio.ListObjectsAsync(args);

            var completionSource = new TaskCompletionSource<bool>();

            observable.Subscribe(
                item => files.Add(item.Key),
                ex => completionSource.SetException(ex),
                () => completionSource.SetResult(true)
            );

            await completionSource.Task;

            return files;
        }

        public async Task UploadAsync(string prefix, IFormFile file)
        {
            var objectName = $"{prefix}/{file.FileName}";

            using var stream = file.OpenReadStream();

            await _minio.PutObjectAsync(new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(file.ContentType));
        }

        public async Task DeleteAsync(string objectName)
        {
            await _minio.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject(objectName));
        }

        public string GetFileUrl(string objectName)
        {
            return $"http://localhost:9000/{BucketName}/{objectName}";
        }
    }
}
