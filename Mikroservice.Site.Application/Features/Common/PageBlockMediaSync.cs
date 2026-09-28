using Mikroservice.Site.Application.DTOs.PageSectionDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.Common
{
    /// <summary>
    /// PageBlock carousel medyalari icin ortak senkronizasyon yardimcisi.
    /// PageBlockMedia'da soft delete alanlari olmadigi icin listeden cikarma (hard delete) uygulanir.
    ///
    /// Create: yeni mediaciklar parent navigation kumesine eklenir (parent Added -> EF cascade ile kaydeder).
    /// Update: mevcut liste TRACKED okunmali; guncelleme/silme SaveChanges ile dusulur,
    ///         yeni mediaciklar collection'a eklenir.
    /// </summary>
    internal static class PageBlockMediaSync
    {
        public static void Apply(
            ICollection<PageBlockMedia> mevcutMedias,
            List<PageBlockMediaInputDto>? girdiler)
        {
            girdiler ??= [];

            // Girdilerde Id bulunmadigindan tum mevcut mediaciklar silinip yeniden olusturulur.
            // (Basit ve tutarli strateji: her save'de carousel slaytlari formdan gelen listeyle birebir esitlenir.)
            foreach (var mevcut in mevcutMedias.ToList())
            {
                mevcutMedias.Remove(mevcut);
            }

            foreach (var girdi in girdiler)
            {
                mevcutMedias.Add(new PageBlockMedia {
                    ResimUrl = girdi.ResimUrl,
                    VideoUrl = girdi.VideoUrl,
                    Sira = girdi.Sira
                });
            }
        }
    }
}
