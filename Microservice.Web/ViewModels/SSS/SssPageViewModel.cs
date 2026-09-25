using Microservice.Web.ViewModels.SikcaSorulanSoru;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.SSS
{
    /// <summary>
    /// SSS (Sikca Sorulan Sorular) sayfasi modeli:
    /// Admin > SikcaSorulanSoru agac yapisi (kokler = kategoriler, Children = sorular).
    /// </summary>
    public class SssPageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Admin > SikcaSorulanSoru agacindan gelen kokler (kategori = ParentId null,
        // Children = sorular).
        public List<SikcaSorulanSoruVm> SoruAgaci { get; set; } = new();
    }
}
