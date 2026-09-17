namespace Mikroservice.Site.Application.DTOs.IcerikResimDtos
{
    /// <summary>
    /// Create/Update komutlarinda gonderilen galeri resmi girdisi.
    /// Id degeri 0 ise yeni resim, deger varsa mevcut resmin guncellemesi olarak islenir.
    /// </summary>
    public class IcerikResimInputDto
    {
        public int Id { get; set; }
        public string? Baslik { get; set; }
        public string ResimUrl { get; set; } = default!;
        public int Sira { get; set; }
    }
}
