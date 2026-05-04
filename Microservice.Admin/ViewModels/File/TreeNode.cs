namespace Microservice.Admin.ViewModels.File
{
    public class TreeNode
    {
        public string Title { get; set; } = default!;
        public string Key { get; set; } = default!;
        public bool Folder { get; set; }
        public long Size { get; set; }
        // Sadece klasörler lazy (açılabilir) olmalı
        public bool Lazy => Folder;
    }
}
