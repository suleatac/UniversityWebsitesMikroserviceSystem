using Mikroservice.Site.Application.DTOs.IcerikDosyaDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.Common
{
    /// <summary>
    /// Icerik (Haber/Duyuru) ek dosyalarini, formdan gelen girdilerle esitleyen ortak yardimci.
    /// Id = 0 -> yeni dosya (dondurulan listede), Id > 0 -> mevcut dosya guncelleme,
    /// listede olmayan mevcut dosya -> soft delete.
    ///
    /// Create: dondurulen yeni dosyalar parent'in navigation kumesine eklenir (parent Added oldugu icin EF izler).
    /// Update: mevcut liste repository'den TRACKED okunmali; guncelleme/silme SaveChanges ile dusulur,
    ///         dondurulen yeni dosyalar ayrica AddAsync edilmelidir.
    /// </summary>
    internal static class IcerikDosyaSync
    {
        public static List<IcerikDosya> Apply(
            ICollection<IcerikDosya> mevcutDosyalar,
            List<IcerikDosyaInputDto>? girdiler,
            int icerikId)
        {
            var yeniDosyalar = new List<IcerikDosya>();
            girdiler ??= new List<IcerikDosyaInputDto>();

            var gelenIdler = girdiler
                .Where(g => g.Id > 0)
                .Select(g => g.Id)
                .ToHashSet();

            // 1) Listede olmayan mevcut (aktif) dosyalari soft delete et
            foreach (var mevcut in mevcutDosyalar.Where(d => !d.IsDeleted).ToList())
            {
                if (!gelenIdler.Contains(mevcut.Id))
                    mevcut.IsDeleted = true;
            }

            // 2) Girdileri isle
            foreach (var girdi in girdiler)
            {
                if (girdi.Id > 0)
                {
                    var mevcut = mevcutDosyalar.FirstOrDefault(d => d.Id == girdi.Id);
                    if (mevcut is null)
                        continue;

                    mevcut.Baslik = girdi.Baslik;
                    mevcut.DosyaUrl = girdi.DosyaUrl;
                    mevcut.DosyaAdi = girdi.DosyaAdi;
                    mevcut.DosyaBoyut = girdi.DosyaBoyut;
                    mevcut.DosyaTuru = girdi.DosyaTuru;
                    mevcut.Sira = girdi.Sira;
                    mevcut.IsDeleted = false; // onceki taslakta silindiyse geri getir
                }
                else
                {
                    yeniDosyalar.Add(new IcerikDosya
                    {
                        IcerikId = icerikId,
                        Baslik = girdi.Baslik,
                        DosyaUrl = girdi.DosyaUrl,
                        DosyaAdi = girdi.DosyaAdi,
                        DosyaBoyut = girdi.DosyaBoyut,
                        DosyaTuru = girdi.DosyaTuru,
                        Sira = girdi.Sira,
                        YuklemeTarihi = DateTime.Now,
                        IsDeleted = false
                    });
                }
            }

            return yeniDosyalar;
        }
    }
}
