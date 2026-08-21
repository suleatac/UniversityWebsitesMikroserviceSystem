namespace Mikroservice.Site.Domain.Entities
{
    public class PageType
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public ICollection<PageRoute> PageRoutes { get; set; } = new List<PageRoute>();


    }
}
