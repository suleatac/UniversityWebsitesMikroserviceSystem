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

        // 1. ADIM: Ebeveyn (Parent) dizini doğru hesapla
        // "site/1/haberler/" -> TrimEnd('/') ile "site/1/haberler" yaparız.
        // Sonra son '/' işaretinden öncesini alarak "site/1/" ebeveyn yolunu buluruz.
        var trimmedOldPath = oldPath.TrimEnd('/');
        var lastSlashIndex = trimmedOldPath.LastIndexOf('/');
        var parentPath = lastSlashIndex >= 0 ? trimmedOldPath.Substring(0, lastSlashIndex + 1) : "";

        // Yeni yolu oluştur (Eğer klasörse sonuna / ekle)
        var newPath = parentPath + newName.Trim();
        if (oldPath.EndsWith("/")) newPath += "/";

        // 2. ADIM: Eğer bu bir klasörse içindeki TÜM nesneleri taşı
        if (oldPath.EndsWith("/"))
        {
            // Klasörün içindeki her şeyi listele (Recursive: true)
            var listArgs = new ListObjectsArgs()
                .WithBucket(_settings.BucketName)
                .WithPrefix(oldPath)
                .WithRecursive(true);

            var objectsToMove = new List<string>();
            var tcs = new TaskCompletionSource<bool>();

            _minio.ListObjectsAsync(listArgs).Subscribe(
                item => objectsToMove.Add(item.Key),
                ex => tcs.SetException(ex),
                () => tcs.SetResult(true)
            );

            await tcs.Task;

            // Her bir alt dosyayı yeni prefix ile kopyala ve eskiyi sil
            foreach (var oldObjectKey in objectsToMove)
            {
                // Örn: site/1/haberler/resim.jpg -> site/1/news/resim.jpg
                var newObjectKey = oldObjectKey.Replace(oldPath, newPath);

                await CopyObject(oldObjectKey, newObjectKey);
                await _minio.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(_settings.BucketName)
                    .WithObject(oldObjectKey));
            }
        }
        else
        {
            // 3. ADIM: Eğer bu sadece bir dosyaysa direkt taşı
            await CopyObject(oldPath, newPath);
            await DeleteAsync(oldPath, siteId);
        }
    }

    // ✅ MOVE METODUNU DÜZELT (Unique isim garantisi)
    public async Task MoveAsync(string source, string target, int siteId)
    {
        ValidatePath(source, siteId);
        ValidatePath(target, siteId);

        if (!target.EndsWith("/")) target += "/";

        if (source.EndsWith("/")) // KLASÖR TAŞIMA
        {
            var folderName = source.TrimEnd('/').Split('/').Last();
            var newFolderPath = await GetUniqueFolderName($"{target}{folderName}/");

            // Rename ile taşı (içindekilerle beraber)
            await RenameAsync(source, newFolderPath.TrimEnd('/').Split('/').Last(), siteId);
        }
        else // DOSYA TAŞIMA
        {
            var fileName = Path.GetFileName(source);
            var uniqueTarget = await GetUniqueFileNameAsync(target, fileName, siteId);
            var newPath = $"{target}{uniqueTarget}";

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
                finalPath = await GetUniqueFileNameAsync(target, $"{baseName}_copy{ext}", siteId);
            }
            else
            {
                // FARKLI KLASÖR: Benzersiz isim + TAM YOL
                var uniqueFileName = await GetUniqueFileNameAsync(target, fileName, siteId);
                finalPath = $"{target}{uniqueFileName}";  // ✅ TAM YOL!
            }

            await CopyObject(source, finalPath);
        }
        else // KLASÖR KOPYALA
        {
            var folderName = source.TrimEnd('/').Split('/').Last();
            var sourceParent = source.Substring(0, source.LastIndexOf('/'));
            var newFolderPath = await GetUniqueFolderName($"{target}{folderName}/");

            // Boş klasör oluştur
            await CreateEmptyFolder(newFolderPath);

            // İçeriği kopyala
            var listArgs = new ListObjectsArgs()
                .WithBucket(_settings.BucketName)
                .WithPrefix(source)
                .WithRecursive(true);

            var objects = new List<string>();
            var tcs = new TaskCompletionSource<bool>();

            _minio.ListObjectsAsync(listArgs).Subscribe(
                item => { if (item.Key != source) objects.Add(item.Key); },
                ex => tcs.SetException(ex),
                () => tcs.SetResult(true)
            );

            await tcs.Task;

            var copyTasks = objects.Select(obj => {
                var relativePath = obj.Replace(source, "");
                return CopyObject(obj, newFolderPath + relativePath);
            });

            await Task.WhenAll(copyTasks);
        }
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
        // Eğer klasör (veya dummy objesi) YOKSA orijinal ismi döndür
        if (!await ObjectExists(folderPath))
            return folderPath;

        var basePath = folderPath.TrimEnd('/');
        var parent = basePath.Substring(0, basePath.LastIndexOf('/') + 1);
        var name = basePath.Substring(parent.Length);

        for (int counter = 1; counter <= 50; counter++)
        {
            string newPath = $"{parent}{name} ({counter})/";
            if (!await ObjectExists(newPath)) return newPath;
        }
        return $"{parent}{name}_{DateTime.UtcNow.Ticks}/";
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