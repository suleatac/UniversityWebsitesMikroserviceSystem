using Microservice.Admin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly MinioService _service;

        public FileController(MinioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int siteId)
        {
            var prefix = $"site{siteId}";
            var files = await _service.GetFilesAsync(prefix);

            return Ok(files.Select(f => new {
                name = Path.GetFileName(f),
                fullPath = f,
                url = _service.GetFileUrl(f)
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromQuery] int siteId, List<IFormFile> files)
        {
            var prefix = $"site{siteId}";

            foreach (var file in files)
            {
                await _service.UploadAsync(prefix, file);
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string path)
        {
            await _service.DeleteAsync(path);
            return Ok();
        }
    }
}
