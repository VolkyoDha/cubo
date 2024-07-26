using System.IO;
using System.Linq;
using MovieDashboard.Data;

namespace MovieDashboard.Services
{
    public class CsvExportService
    {
        private readonly AppDbContext _context;

        public CsvExportService(AppDbContext context)
        {
            _context = context;
        }

        public void ExportMoviesToCsv(string filePath)
        {
            var movies = _context.FactMovies.Select(m => new
            {
                Title = m.Movie.Title,
                Director = m.Director.Name,
                Genre = m.Genre.Name,
                Year = m.Year,
                Duration = m.Duration,
                Rating = m.Rating,
                RatingCount = m.RatingCount,
                ImdbLink = m.Movie.ImdbLink
            }).ToList();

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Title,Director,Genre,Year,Duration,Rating,RatingCount,ImdbLink");

                foreach (var movie in movies)
                {
                    writer.WriteLine($"{movie.Title},{movie.Director},{movie.Genre},{movie.Year},{movie.Duration},{movie.Rating},{movie.RatingCount},{movie.ImdbLink}");
                }
            }
        }
    }
}
