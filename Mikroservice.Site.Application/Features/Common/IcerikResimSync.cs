using Mikroservice.Site.Application.DTOs.IcerikResimDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.Common
{
    /// <summary>
    /// Icerik (Haber/Duyuru) galeri resimlerini, formdan gelen girdilerle esitleyen ortak yardimci.
    /// Id = 0 -> yeni resim (dondurulan listede), Id > 0 -> mevcut resim guncelleme,
    /// listede olmayan mevcut resim -> soft delete.
    ///
    /// Create: dondurulen yeni resimler parent'in navigation kumesine eklenir (parent Added oldugu icin EF izler).
    /// Update: mevcut liste repository'den TRACKED okunmali; guncelleme/silme SaveChanges ile dusulur,
    ///         dondurulen yeni resimler ayrica AddAsync edilmelidir.
    /// </summary>
    internal static class IcerikResimSync
    {
        public static List<IcerikResim> Apply(
            ICollection<IcerikResim> mevcutResimler,
            List<IcerikResimInputDto>? girdiler,
            int icerikId)
        {
            var yeniResimler = new List<IcerikResim>();
            girdiler ??= new List<IcerikResimInputDto>();

            var gelenIdler = girdiler
                .Where(g => g.Id > 0)
                .Select(g => g.Id)
                .ToHashSet();

            // 1) Listede olmayan mevcut (aktif) resimleri soft delete et
            foreach (var mevcut in mevcutResimler.Where(r => !r.IsDeleted).ToList())
            {
                if (!gelenIdler.Contains(mevcut.Id))
                    mevcut.IsDeleted = true;
            }

            // 2) Girdileri isle
            foreach (var girdi in girdiler)
            {
                if (girdi.Id > 0)
                {
                    var mevcut = mevcutResimler.FirstOrDefault(r => r.Id == girdi.Id);
                    if (mevcut is null)
                        continue;

                    mevcut.Baslik = girdi.Baslik;
                    mevcut.ResimUrl = girdi.ResimUrl;
                    mevcut.Sira = girdi.Sira;
                    mevcut.IsDeleted = false; // onceki taslakta silindiyse geri getir
                }
                else
                {
                    yeniResimler.Add(new IcerikResim
                    {
                        IcerikId = icerikId,
                        Baslik = girdi.Baslik,
                        ResimUrl = girdi.ResimUrl,
                        Sira = girdi.Sira,
                        YuklemeTarihi = DateTime.Now,
                        IsDeleted = false
                    });
                }
            }

            return yeniResimler;
        }
    }
}
