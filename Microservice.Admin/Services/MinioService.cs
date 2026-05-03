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
        // Path null ise kök dizini ayarla, değilse path'in sonunun / ile bittiğinden emin ol
        var prefix = path ?? $"site/{siteId}/";
        if (!prefix.EndsWith("/")) prefix += "/";

        var nodes = new List<TreeNode>();

        var args = new ListObjectsArgs()
            .WithBucket(_settings.BucketName)
            .WithPrefix(prefix)
            .WithRecursive(false);

        var tcs = new TaskCompletionSource<bool>();

        var subscription = _minio.ListObjectsAsync(args).Subscribe(
            item => {
                // ÖNEMLİ: Eğer gelen item'ın key'i sorguladığımız prefix ile aynıysa (klasörün kendisi), listeye ekleme
                if (item.Key == prefix) return;

                // İsim hesaplama: prefix'ten sonrasını al ve sondaki / işaretini at
                var title = item.Key.Substring(prefix.Length).TrimEnd('/');

                // Eğer title boş kalmışsa (yanlışlıkla oluşmuş objeler için koruma) ekleme
                if (string.IsNullOrEmpty(title)) return;

                nodes.Add(new TreeNode
                {
                    Title = title,
                    Key = item.Key,
                    Folder = item.Key.EndsWith("/")
                });
            },
            ex => tcs.SetException(ex),
            () => tcs.SetResult(true)
        );

        await tcs.Task;
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
            // GUID yerine orijinal ismi al
            var fileName = file.FileName;
            var fullPath = $"{path}{fileName}";

            // Eğer bu isimde dosya varsa benzersiz yap, yoksa orijinal ismi kullan
            var objectName = await GetUniqueObjectName(fullPath);

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
        // Güvenlik kontrolü
        ValidatePath(path, siteId);

        // Eğer yol '/' ile bitiyorsa bu bir klasördür
        if (path.EndsWith("/"))
        {
            // 1. Klasörün içindeki TÜM nesneleri (dosyalar ve alt klasörler) listele
            var listArgs = new ListObjectsArgs()
                .WithBucket(_settings.BucketName)
                .WithPrefix(path)
                .WithRecursive(true); // İçindeki her şeyi bulması için true

            var objectsToDelete = new List<string>();
            var tcs = new TaskCompletionSource<bool>();

            // Listeleme işlemi
            var subscription = _minio.ListObjectsAsync(listArgs).Subscribe(
                item => objectsToDelete.Add(item.Key),
                ex => tcs.SetException(ex),
                () => tcs.SetResult(true)
            );

            await tcs.Task;

            // 2. Bulunan tüm nesneleri tek tek sil
            // Not: Çok fazla dosya varsa 'RemoveObjectsAsync' (toplu silme) daha performanslıdır 
            // ancak basitlik için bu yöntem de iş görür.
            foreach (var objKey in objectsToDelete)
            {
                await _minio.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(_settings.BucketName)
                    .WithObject(objKey));
            }
        }
        else
        {
            // Eğer bir dosyaysa direkt sil
            await _minio.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(path));
        }
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

        // Yeni tam yol oluştur
        var parentPath = oldPath.TrimEnd('/').Substring(0, oldPath.TrimEnd('/').LastIndexOf('/') + 1);
        var newPath = $"{parentPath}{newName}";
        if (oldPath.EndsWith("/")) newPath += "/";

        Console.WriteLine($"Rename: {oldPath} → {newPath}");

        if (oldPath.EndsWith("/")) // KLASÖR
        {
            // Tüm içerikleri listele
            var contents = await ListFolderContents(oldPath);

            // Her birini yeni yola taşı
            foreach (var oldObjectKey in contents)
            {
                var relativePath = oldObjectKey.Replace(oldPath, "");
                var newObjectKey = newPath + relativePath;

                // ✅ Farklı yol kontrolü
                if (oldObjectKey != newObjectKey)
                {
                    await CopyObject(oldObjectKey, newObjectKey);
                    await _minio.RemoveObjectAsync(new RemoveObjectArgs()
                        .WithBucket(_settings.BucketName)
                        .WithObject(oldObjectKey));
                }
            }

            // Eski placeholder'ı sil
            await _minio.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(oldPath));
        }
        else // DOSYA
        {
            if (oldPath != newPath)
            {
                await CopyObject(oldPath, newPath);
                await DeleteAsync(oldPath, siteId);
            }
        }

        // Yeni placeholder oluştur
        if (newPath.EndsWith("/"))
        {
            await CreateEmptyFolder(newPath);
        }
    }

    // ✅ MOVE METODUNU DÜZELT (Unique isim garantisi)
    public async Task MoveAsync(string source, string targetFolder, int siteId)
    {
        ValidatePath(source, siteId);
        ValidatePath(targetFolder, siteId);

        if (!targetFolder.EndsWith("/")) targetFolder += "/";

        if (source.EndsWith("/")) // KLASÖR TAŞIMA
        {
            // ✅ RenameAsync ile taşı (CopyObject kullanMA!)
            var folderName = source.TrimEnd('/').Split('/').Last();
            var uniqueFolderName = await GetUniqueFolderName($"{targetFolder}{folderName}/");
            var newFolderName = uniqueFolderName.TrimEnd('/').Split('/').Last(); // Sadece isim

            await RenameAsync(source, newFolderName, siteId);  // ✅ Bu copy+delete yapar
        }
        else // DOSYA TAŞIMA
        {
            var fileName = Path.GetFileName(source);
            var uniqueFileName = await GetUniqueFileNameAsync(targetFolder, fileName, siteId);
            var newPath = $"{targetFolder}{uniqueFileName}";

            await CopyObject(source, newPath);
            await DeleteAsync(source, siteId);
        }
    }


    public async Task CreateFolderAsync(string path, string name, int siteId)
    {
        ValidatePath(path, siteId);

        // Ana yolun sonuna / ekle
        if (!path.EndsWith("/")) path += "/";

        // Yeni klasör yolunu oluştur ve sonuna / ekle (MinIO klasör olduğunu anlasın)
        var folderPath = $"{path}{name.Trim()}/";

        // 0 byte yerine 1 byte'lık boşluk (space) kullanıyoruz.
        // Bu, "ObjectSize must be set" hatasını ve bazı SDK sürümlerindeki 0-byte hatalarını önler.
        byte[] content = new byte[] { 32 }; // 32 = Boşluk karakteri (ASCII Space)

        using (var memoryStream = new MemoryStream(content))
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(folderPath)
                .WithStreamData(memoryStream)
                .WithObjectSize(memoryStream.Length) // 1 byte olduğunu buradan açıkça alır
                .WithContentType("application/x-directory"); // Opsiyonel: Klasör tipi olduğunu belirtir

            await _minio.PutObjectAsync(putObjectArgs);
        }
    }

    // ✅ DOSYA ADI VAR MI KONTROLÜ (Optimize)
    public async Task<bool> FileNameExistsAsync(string folderPath, string fileName, int siteId)
    {
        ValidatePath(folderPath, siteId);
        if (!folderPath.EndsWith("/")) folderPath += "/";
        var fullPath = $"{folderPath}{fileName}";

        var listArgs = new ListObjectsArgs()
            .WithBucket(_settings.BucketName)
            .WithPrefix(fullPath)
            .WithRecursive(false);

        var objects = new List<string>();
        var tcs = new TaskCompletionSource<bool>();

        // ✅ Subscription dispose edilebilir
        using var subscription = _minio.ListObjectsAsync(listArgs).Subscribe(
            item => {
                objects.Add(item.Key);
            },
            ex => {
                tcs.TrySetException(ex);
            },
            () => {
                tcs.TrySetResult(objects.Any(obj => obj == fullPath));
            }
        );

        // ✅ 5 saniye timeout
        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
        var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

        if (completedTask == timeoutTask)
        {
            return false; // Timeout = yok say
        }

        return await tcs.Task;
    }

    // ✅ YENİ: BENZERSİZ DOSYA ADI ÜRETME (Sonsuz döngü yok!)
    public async Task<string> GetUniqueFileNameAsync(string folderPath, string originalName, int siteId)
    {
        ValidatePath(folderPath, siteId);
        if (!folderPath.EndsWith("/")) folderPath += "/";

        if (!await FileNameExistsAsync(folderPath, originalName, siteId))
            return originalName;

        var baseName = Path.GetFileNameWithoutExtension(originalName);
        var ext = Path.GetExtension(originalName);

        // ✅ MAX 100 DENEME (sonsuz döngü yok)
        for (int i = 1; i <= 100; i++)
        {
            var newName = $"{baseName} ({i}){ext}";
            if (!await FileNameExistsAsync(folderPath, newName, siteId))
                return newName;
        }

        // ✅ Son çare: timestamp
        var timestamp = DateTime.UtcNow.Ticks;
        return $"{folderPath}{baseName}_{timestamp}{ext}";
    }
    // ✅ COPY METODUNU DÜZELT (Aynı klasör kontrolü)
    public async Task CopyAsync(string source, string target, int siteId)
    {
        ValidatePath(source, siteId);
        ValidatePath(target, siteId);

        if (!target.EndsWith("/")) target += "/";

        if (!source.EndsWith("/")) // DOSYA KOPYALA
        {
            var fileName = Path.GetFileName(source);
            var sourceFolder = source.Substring(0, source.LastIndexOf('/') + 1);

            string finalPath;
            if (sourceFolder == target)
            {
                // ✅ AYNI KLASÖR: _copy ekle
                var baseName = Path.GetFileNameWithoutExtension(fileName);
                var ext = Path.GetExtension(fileName);
                var uniqueFilesName = await GetUniqueFileNameAsync(target, $"{baseName}_copy{ext}", siteId);
                finalPath = $"{target}{uniqueFilesName}";  // ✅ TAM YOL!
            }
            else
            {
                // FARKLI KLASÖR: Benzersiz isim + TAM YOL
                var uniqueFileName = await GetUniqueFileNameAsync(target, fileName, siteId);
                finalPath = $"{target}{uniqueFileName}";  // ✅ TAM YOL!
            }

            await CopyObject(source, finalPath);
        }
        else // KLASÖR KOPYALAMA
        {
            var folderName = source.TrimEnd('/').Split('/').Last();
            var newFolderPath = await GetUniqueFolderName($"{target}{folderName}/");

            // ✅ Klasör placeholder oluştur (zorunlu!)
            await CreateEmptyFolder(newFolderPath);

            // İçeriği kopyala
            var contents = await ListFolderContents(source);
            foreach (var item in contents)
            {
                var relativePath = item.Replace(source, "");
                await CopyObject(item, newFolderPath + relativePath);
            }
        }
    }

    // Helper: Klasör içindekileri listele
    private async Task<List<string>> ListFolderContents(string folderPath)
    {
        var listArgs = new ListObjectsArgs()
            .WithBucket(_settings.BucketName)
            .WithPrefix(folderPath)
            .WithRecursive(true);

        var contents = new List<string>();
        var tcs = new TaskCompletionSource<bool>();

        _minio.ListObjectsAsync(listArgs).Subscribe(
            item => contents.Add(item.Key),
            ex => tcs.SetException(ex),
            () => tcs.SetResult(true)
        );

        await tcs.Task;
        return contents;
    }
    private async Task CreateEmptyFolder(string folderPath)
    {
        byte[] content = new byte[] { 32 };
        using var memoryStream = new MemoryStream(content);

        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(folderPath)
            .WithStreamData(memoryStream)
            .WithObjectSize(1)
            .WithContentType("application/x-directory"));
    }

    public async Task CopyMultipleAsync(List<(string source, string target)> items, int siteId)
    {
        var tasks = items.Select(item => CopyAsync(item.source, item.target, siteId));
        await Task.WhenAll(tasks);
    }
    private async Task<string> GetUniqueObjectName(string objectName)
    {
        // Eğer hedefte bu dosya YOKSA, direkt ismi döndür (Sayı ekleme!)
        if (!await ObjectExists(objectName))
            return objectName;

        // Sadece dosya varsa buraya girer:
        var dir = objectName.Substring(0, objectName.LastIndexOf('/') + 1);
        var fileName = Path.GetFileName(objectName);
        var baseName = Path.GetFileNameWithoutExtension(fileName);
        var ext = Path.GetExtension(fileName);

        for (int counter = 1; counter <= 50; counter++)
        {
            string newName = $"{dir}{baseName} ({counter}){ext}";
            if (!await ObjectExists(newName)) return newName;
        }
        return $"{dir}{baseName}_{DateTime.UtcNow.Ticks}{ext}";
    }
    private async Task<string> GetUniqueFolderName(string folderPath)
    {
        // ✅ KLASÖR KONTROLÜ
        if (!await FolderExistsAsync(folderPath))
            return folderPath;

        var basePath = folderPath.TrimEnd('/');
        var parent = basePath.Substring(0, basePath.LastIndexOf('/') + 1);
        var name = basePath.Substring(parent.Length);

        // (1), (2)... dene
        for (int counter = 1; counter <= 50; counter++)
        {
            var newPath = $"{parent}{name} ({counter})/";
            if (!await FolderExistsAsync(newPath))
                return newPath;
        }

        // Timestamp fallback
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        return $"{parent}{name}_{timestamp}/";
    }

    private async Task<bool> FolderExistsAsync(string folderPath)
    {
        if (!folderPath.EndsWith("/")) folderPath += "/";

        // ✅ KLASÖR İÇİNDE OBJE VAR MI KONTROL ET
        var listArgs = new ListObjectsArgs()
            .WithBucket(_settings.BucketName)
            .WithPrefix(folderPath)
            .WithRecursive(false);

        var tcs = new TaskCompletionSource<bool>();
        var hasChildren = false;

        using var subscription = _minio.ListObjectsAsync(listArgs).Subscribe(
            item => {
                // Klasörün kendisi hariç, içinde başka obje varsa klasör var say
                if (item.Key != folderPath && item.Key.StartsWith(folderPath))
                {
                    hasChildren = true;
                    tcs.TrySetResult(true);
                }
            },
            ex => tcs.TrySetException(ex),
            () => tcs.TrySetResult(hasChildren)
        );

        var timeoutTask = Task.Delay(3000);
        var result = await Task.WhenAny(tcs.Task, timeoutTask);
        return result != timeoutTask && await tcs.Task;
    }
    // ✅ DAHA SAĞLAM EXIST KONTROLÜ (Timeout 100ms'den 2 saniyeye çıkarıldı)
    // ✅ GENEL OBJECT VAR MI KONTROLÜ (Timeout korumalı)
    private async Task<bool> ObjectExists(string objectName)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            await _minio.StatObjectAsync(new StatObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(objectName), cts.Token);
            return true;
        }
        catch
        {
            return false;
        }
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