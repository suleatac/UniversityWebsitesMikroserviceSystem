namespace Microservice.Admin.ViewModels.File
{
    public class TreeNode
    {
        public string Title { get; set; } = default!;
        public string Key { get; set; } = default!;
        public bool Folder { get; set; }
        public bool Lazy { get; set; } = true;
    }
}
