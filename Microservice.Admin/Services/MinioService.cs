using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Settings;
using Microservice.Admin.ViewModels.File;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

public class MinioService : IMinioService
{
    private readonly IMinioClient _minio;
    private readonly MinioSetting _settings;

    public MinioService(IMinioClient minio, IOptions<MinioSetting> settings)
    {
        _minio = minio;
        _settings = settings.Value;
    }

    public async Task<List<TreeNode>> GetTreeAsync(int siteId, string? path)
    {
        var prefix = path ?? $"site/{siteId}/";
        var nodes = new List<TreeNode>();

        var args = new ListObjectsArgs()
            .WithBucket(_settings.BucketName)
            .WithPrefix(prefix)
            .WithRecursive(false);

        var tcs = new TaskCompletionSource<bool>();

        // Subscribe ile her bir item geldiğinde listeye ekliyoruz
        var subscription = _minio.ListObjectsAsync(args).Subscribe(
            item => {
                nodes.Add(new TreeNode
                {
                    Title = item.IsDir
                        ? item.Key.Replace(prefix, "").TrimEnd('/')
                        : Path.GetFileName(item.Key),
                    Key = item.Key,
                    Folder = item.IsDir
                });
            },
            ex => tcs.SetException(ex),    // Hata oluşursa görevi iptal et
            () => tcs.SetResult(true)      // İşlem bittiğinde devam et
        );

        await tcs.Task; // Tüm öğeler okunana kadar asenkron olarak bekler
        return nodes;
    }

    public async Task<string> UploadAsync(IFormFile file, int siteId, string module)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var objectName = $"site/{siteId}/{module}/{fileName}";

        using var stream = file.OpenReadStream();

        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType));

        return objectName;
    }

    public async Task UploadMultipleAsync(List<IFormFile> files, string path)
    {
        foreach (var file in files)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var objectName = $"{path}{fileName}";

            using var stream = file.OpenReadStream();

            await _minio.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType));
        }
    }

    public async Task DeleteAsync(string path, int siteId)
    {
        // Güvenlik: Kullanıcının sadece kendi site klasöründeki dosyaları silmesini sağla
        ValidatePath(path, siteId);


        await _minio.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(path));
    }

    public async Task DeleteMultipleAsync(List<string> paths, int siteId)
    {
        foreach (var path in paths)
        {
            await DeleteAsync(path, siteId);
        }
    }

    public async Task RenameAsync(string oldPath, string newName, int siteId)
    {
        ValidatePath(oldPath, siteId);

        var newPath = Path.GetDirectoryName(oldPath) + "/" + newName;

        await CopyObject(oldPath, newPath);
        await DeleteAsync(oldPath, siteId);
    }

    public async Task MoveAsync(string source, string target, int siteId)
    {
        ValidatePath(source, siteId);
        ValidatePath(target, siteId);

        var fileName = Path.GetFileName(source);
        var newPath = $"{target}{fileName}";

        await CopyObject(source, newPath);
        await DeleteAsync(source, siteId);
    }

    public async Task CreateFolderAsync(string path, string name, int siteId)
    {
        ValidatePath(path, siteId);

        var folderPath = $"{path}{name}/";

        using var stream = new MemoryStream();

        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(folderPath)
            .WithStreamData(stream)
            .WithObjectSize(0));
    }

    private async Task CopyObject(string source, string destination)
    {
        await _minio.CopyObjectAsync(new CopyObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(destination)
            .WithCopyObjectSource(
                new CopySourceObjectArgs()
                    .WithBucket(_settings.BucketName)
                    .WithObject(source)));
    }

    // 1. Güvenlik ve Yol Doğrulama Metodu
    private void ValidatePath(string path, int siteId)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Yol boş olamaz.");

        var prefix = $"site/{siteId}/";

        // Eğer yol belirtilen siteId'ye ait prefix ile başlamıyorsa işlemi engelle
        if (!path.StartsWith(prefix))
        {
            throw new UnauthorizedAccessException($"Hatalı erişim: {path} yolu site {siteId} için yetkili değil.");
        }
    }


}