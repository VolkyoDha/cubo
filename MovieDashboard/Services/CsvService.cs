using System.Linq;
using MovieDashboard.Data;
using MovieDashboard.Models;
using System.IO;
using System.Collections.Generic; // Asegúrate de incluir esta línea
using Microsoft.EntityFrameworkCore; // Asegúrate de incluir esta línea

namespace MovieDashboard.Services
{
    public class CsvService
    {
        private readonly AppDbContext _context;

        public CsvService(AppDbContext context)
        {
            _context = context;
        }

        public void LoadMoviesFromCsv(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++) // Comenzar desde la segunda línea (índice 1)
            {
                var line = lines[i];
                var values = line.Split(',');

                // Comprobación de que la línea tiene al menos 9 columnas
                if (values.Length < 9)
                {
                    // Log de depuración para líneas con formato incorrecto
                    Console.WriteLine($"Línea {i + 1} tiene un número inesperado de columnas: {values.Length}");
                    Console.WriteLine($"Contenido de la línea: {line}");
                    continue;
                }

                try
                {
                    var movie = _context.DimMovies.FirstOrDefault(m => m.Title == values[1]) ??
                                new DimMovie { Title = values[1], ImdbLink = values.Length > 9 ? values[9] : string.Empty };

                    var director = _context.DimDirectors.FirstOrDefault(d => d.Name == values[6]) ??
                                   new DimDirector { Name = values[6] };

                    var genre = _context.DimGenres.FirstOrDefault(g => g.Name == values[3]) ??
                                new DimGenre { Name = values[3] };

                    var existingFactMovie = _context.FactMovies.FirstOrDefault(fm => fm.Movie.Title == movie.Title && fm.Director.Name == director.Name && fm.Year == int.Parse(values[2]));

                    if (existingFactMovie != null)
                    {
                        // Update existing record
                        existingFactMovie.Duration = ConvertDuration(values[4]);
                        existingFactMovie.Rating = float.Parse(values[7]);
                        existingFactMovie.RatingCount = int.Parse(values[8]);
                    }
                    else
                    {
                        // Insert new record
                        var factMovie = new FactMovie
                        {
                            Movie = movie,
                            Director = director,
                            Genre = genre,
                            Year = int.Parse(values[2]),
                            Duration = ConvertDuration(values[4]),
                            Rating = float.Parse(values[7]),
                            RatingCount = int.Parse(values[8])
                        };

                        _context.FactMovies.Add(factMovie);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al procesar la línea {i + 1}: {ex.Message}");
                    Console.WriteLine($"Contenido de la línea: {line}");
                }
            }

            _context.SaveChanges();
        }

        private int ConvertDuration(string duration)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(duration))
                    return 0;

                int hours = 0;
                int minutes = 0;

                if (duration.Contains("h"))
                {
                    var parts = duration.Split('h');
                    hours = int.Parse(parts[0]);
                    if (parts.Length > 1 && parts[1].Contains("min"))
                    {
                        minutes = int.Parse(parts[1].Replace("min", "").Trim());
                    }
                }
                else if (duration.Contains("min"))
                {
                    minutes = int.Parse(duration.Replace("min", "").Trim());
                }

                return hours * 60 + minutes;
            }
            catch
            {
                Console.WriteLine($"Formato de duración no válido: {duration}");
                return 0; // En caso de error, devuelve 0 minutos.
            }
        }

        public IEnumerable<FactMovie> GetAllMovies()
        {
            return _context.FactMovies.Include(fm => fm.Movie).Include(fm => fm.Director).Include(fm => fm.Genre).ToList();
        }
    }
}
