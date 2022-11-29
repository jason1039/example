using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using example.Models;

namespace example.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHostEnvironment _hostingEnvironment;
    public HomeController(ILogger<HomeController> logger, IHostEnvironment hostingEnvironment)
    {
        _logger = logger;
        _hostingEnvironment = hostingEnvironment;
    }


    // public HomeController(IHostEnvironment hostingEnvironment)
    // {
    //     _hostingEnvironment = hostingEnvironment;
    // }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public JsonResult UploadFile()
    {
        IFormFile File = Request.Form.Files[0];
        Microsoft.Extensions.Primitives.StringValues fileName = string.Empty;
        Request.Form.TryGetValue("fileName", out fileName);
        string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads");
        //Create directory if it doesn't exist 
        Directory.CreateDirectory(uploads);
        if (File.Length > 0)
        {
            string filePath = Path.Combine(uploads, fileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                File.CopyTo(fileStream);
            }
        }
        return Json("Success");
    }
    [HttpPost]
    public JsonResult mkFile()
    {
        string rootPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads");
        List<string> files = Directory.GetFiles(rootPath).ToList()
        .FindAll(
            delegate (string fileName)
            {
                return fileName.IndexOf("test") != -1;
            }
            );
        using (var outputStream = System.IO.File.Create("test.xlsx"))
        {
            for (int i = 0; i < files.Count; i++)
            {
                using (var inputStream = System.IO.File.OpenRead(Path.Combine(rootPath, $"test_{i}")))
                {
                    inputStream.CopyTo(outputStream);
                }
            }
        }

        return Json("Success");
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
