namespace Microservice.Admin.ViewModels.Banner
{
    public class ReorderBannerItemVm
    {
        public int Id { get; set; }
        public int Sira { get; set; }
    }

    public class ReorderBannersCommandListVm
    {
        public List<ReorderBannerItemVm> Items { get; set; } = new();
    }
}