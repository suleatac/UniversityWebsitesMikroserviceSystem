namespace Mikroservice.Site.Application.DTOs.BirimDtos
{
    public class BirimDetailDto
    {
        public int Id { get; set; }
        public string Ad { get; set; } = default!;
        public int Sira { get; set; }
        public int? ParentId { get; set; }
    }
}
