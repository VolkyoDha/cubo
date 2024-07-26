using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using MovieDashboard.Services; // Asegúrate de incluir esta línea
using MovieDashboard.Models; // Asegúrate de incluir esta línea

namespace MovieDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly CsvService _csvService;

        public HomeController(CsvService csvService)
        {
            _csvService = csvService;
        }

        [HttpPost]
        public IActionResult UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Content("File not selected");
            }

            // Define the uploads folder path
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            
            // Create the folder if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Define the full file path
            var filePath = Path.Combine(uploadsFolder, file.FileName);

            // Save the file to the uploads folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Process the CSV file
            _csvService.LoadMoviesFromCsv(filePath);

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var movies = _csvService.GetAllMovies();
            return View(movies);
        }
    }
}
