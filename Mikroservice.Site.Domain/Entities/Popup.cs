namespace Mikroservice.Site.Domain.Entities
{
    public class Popup : Icerik
    {
        public bool TamEkranMi { get; set; }=false;
        public int GosterimSuresiSaniye { get; set; }
        public bool CookieIleTekrarGosterme { get; set; }=true;
    }
}
