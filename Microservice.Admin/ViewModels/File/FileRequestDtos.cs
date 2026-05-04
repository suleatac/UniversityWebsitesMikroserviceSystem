namespace Microservice.Admin.ViewModels.File
{
    public class RenameRequest
    {
        public string OldPath { get; set; } = default!;
        public string NewName { get; set; } = default!;
    }

    public class MoveRequest
    {
        public string Source { get; set; } = default!;
        public string Target { get; set; } = default!;
    }

    public class CreateFolderRequest
    {
        public string Path { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
    public class CopyRequest
    {
        public string Source { get; set; } = default!;
        public string Target { get; set; } = default!;
    }
    public class FileConflictRequest
    {
        public List<string> FileNames { get; set; } = default!;
        public string Path { get; set; }
        public int SiteId { get; set; }
    }
}
