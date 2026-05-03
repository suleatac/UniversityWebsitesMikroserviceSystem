using Microservice.Admin.ViewModels.File;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IMinioService
    {
        Task<List<TreeNode>> GetTreeAsync(int siteId, string? path);

        Task<string> UploadAsync(IFormFile file, int siteId, string module);
        Task UploadMultipleAsync(List<IFormFile> files, string path);
        Task CopyAsync(string source, string target, int siteId);
        Task CopyMultipleAsync(List<(string source, string target)> items, int siteId);
        Task DeleteAsync(string path, int siteId);
        Task DeleteMultipleAsync(List<string> paths, int siteId);

        Task RenameAsync(string oldPath, string newName, int siteId);
        Task MoveAsync(string source, string target, int siteId);

        Task CreateFolderAsync(string path, string name, int siteId);
    }
}
