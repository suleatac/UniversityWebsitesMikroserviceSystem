namespace Microservice.Web.ViewModels.Paged
{
    /// <summary>
    /// Mikroservice.Site.Application'daki PaginatedResult<T> karsiligi.
    /// </summary>
    public class PagedResultVm<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}
