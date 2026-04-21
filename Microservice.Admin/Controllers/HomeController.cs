using Microservice.Admin.Models;
using Microservice.Admin.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Microservice.Admin.Controllers
{
    public class HomeController(IMediaService _mediaService) : Controller
    {


        public async Task<IActionResult> Index()
        {

            //Örnet Txt dosyası oluşturma.
            string filePath = "ornek.txt";
            string content = $"Merhaba Dünya!\nTarih: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\nSatır 3";

            // 1. Dosyayı yaz
            await System.IO.File.WriteAllTextAsync(filePath, content);

            // 2. FormFile doğru oluştur
            await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var file = new FormFile(stream, 0, stream.Length, "file", filePath) {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };



            // 3. Upload et
            var url = await _mediaService.UploadAsync(file, 1, "Haberler");


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
