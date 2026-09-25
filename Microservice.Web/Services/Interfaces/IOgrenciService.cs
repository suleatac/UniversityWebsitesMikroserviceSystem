namespace Microservice.Web.Services.Interfaces
{
    public interface IOgrenciService
    {
        /// <summary>
        /// Ogrenci mikroservisindeki kayitlardan ogrenci sayilarini uretir.
        /// Sonuc web tarafinda 10 dk Redis cache ile beslenir.
        /// </summary>
        Task<(int Aktif, int Mezun)> GetOgrenciSayilariAsync();
    }
}
