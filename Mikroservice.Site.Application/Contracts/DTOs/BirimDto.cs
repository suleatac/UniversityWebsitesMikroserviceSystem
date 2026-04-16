namespace Mikroservice.Site.Application.Contracts.DTOs
{
    public class BirimDto
    {
        public int Id { get; set; }
        public string Ad { get; set; } = default!;
        public int Sira { get; set; }
        public int? ParentId { get; set; }

        public List<BirimDto> Children { get; set; } = new();
    }
}
