using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.File;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Route("[controller]")] // Bu satır metodun /File/Tree olarak çalışmasını sağlar
    public class FileController : Controller
    {
        private readonly IMinioService _minioService;

        public FileController(IMinioService minioService)
        {
            _minioService = minioService;
        }
        public async Task<IActionResult> Index(int siteId = 1)
        {
            ViewBag.SiteId=siteId;
            return View();
        }
        [HttpGet("Tree")]
        public async Task<IActionResult> Tree(int siteId, string? path)
        {
            var result = await _minioService.GetTreeAsync(siteId, path);

            // ✅ KÖK NODE İÇİN ÖZEL BAŞLIK
          
            return Json(result);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, int siteId, string module)
        {
            var result = await _minioService.UploadAsync(file, siteId, module);
            return Json(result);
        }

        [HttpPost("CheckConflicts")]
        public async Task<IActionResult> CheckConflicts([FromBody] FileConflictRequest model)
        {
            var conflicts = new List<string>();

            foreach (var fileName in model.FileNames)
            {
                var fullPath = $"{model.Path}{fileName}";

                if (await _minioService.FileNameExistsAsync(model.Path, fileName, model.SiteId))
                {
                    conflicts.Add(fileName);
                }
            }

            return Ok(conflicts);
        }
        [HttpPost("UploadMultiple")]
        public async Task<IActionResult> UploadMultiple(List<IFormFile> files, string path, string mode)
        {
            await _minioService.UploadMultipleAsync(files, path, mode);
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string path, int siteId)
        {
            await _minioService.DeleteAsync(path, siteId);
            return Ok();
        }

        [HttpPost("DeleteMultiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<string> paths, int siteId)
        {
            await _minioService.DeleteMultipleAsync(paths, siteId);
            return Ok();
        }

        [HttpPost("Rename")]
        public async Task<IActionResult> Rename([FromBody] RenameRequest request, int siteId)
        {
            await _minioService.RenameAsync(request.OldPath, request.NewName, siteId);
            return Ok();
        }

        [HttpPost("Move")]
        public async Task<IActionResult> Move([FromBody] MoveRequest request, int siteId)
        {
            await _minioService.MoveAsync(request.Source, request.Target, siteId);
            return Ok();
        }
        [HttpPost("Copy")]
        public async Task<IActionResult> Copy([FromBody] CopyRequest req, int siteId)
        {
            await _minioService.CopyAsync(req.Source, req.Target, siteId);
            return Ok();
        }
        [HttpPost("CopyMultiple")]
        public async Task<IActionResult> CopyMultiple([FromBody] List<CopyRequest> requests, int siteId)
        {
            try
            {
                var tasks = requests.Select(async r =>
                    await _minioService.CopyAsync(r.Source, r.Target, siteId));

                await Task.WhenAll(tasks);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("CreateFolder")]
        public async Task<IActionResult> CreateFolder([FromBody] CreateFolderRequest request, int siteId)
        {
            await _minioService.CreateFolderAsync(request.Path, request.Name, siteId);
            return Ok();
        }
    }
}
