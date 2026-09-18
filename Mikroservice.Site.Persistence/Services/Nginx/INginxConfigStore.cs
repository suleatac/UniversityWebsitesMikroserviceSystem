namespace Mikroservice.Site.Persistence.Services.Nginx
{
    /// <summary>
    /// nginx yapılandırma dosyalarının yazılması/okunması/silinmesinden sorumlu depo soyutlaması.
    /// <para>
    /// Dosya sistemine doğrudan bağımlılığı bu arayüzün arkasına almak,
    /// davranışı birim testlerinde doğrulamayı ve gerektiğinde farklı bir saklama
    /// stratejisine (ör. uzak ajan, git deposu) geçmeyi mümkün kılar.
    /// </para>
    /// </summary>
    public interface INginxConfigStore
    {
        /// <summary>Dosya mevcut mu?</summary>
        bool Exists(string path);

        /// <summary>Dosya içeriğini okur; yoksa null döner.</summary>
        Task<string?> ReadAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// İçeriği atomik olarak yazar. İçerik değişmediyse yazma yapılmaz (idempotency).
        /// </summary>
        /// <returns>Gerçekten yazma yapıldıysa true, içerik zaten aynı olduğu için atlandıysa false.</returns>
        Task<bool> WriteAtomicAsync(string path, string content, CancellationToken cancellationToken = default);

        /// <summary>Dosyayı siler.</summary>
        /// <returns>Dosya bulunup silindiyse true, zaten yoksa false.</returns>
        Task<bool> DeleteAsync(string path, CancellationToken cancellationToken = default);
    }
}
