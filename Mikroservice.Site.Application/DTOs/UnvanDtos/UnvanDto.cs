namespace Mikroservice.Site.Application.DTOs.UnvanDtos
{
    public class UnvanDto
    {
        public int Id { get; set; }
        public int TipId { get; set; }
        public string Ad { get; set; } = default!;
        public string KisaAd { get; set; } = default!;
        public int Sira { get; set; }
        public int? ParentId { get; set; }
        public List<UnvanDto> Children { get; set; } = new();
    }
}
