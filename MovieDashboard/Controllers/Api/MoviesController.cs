using Microsoft.AspNetCore.Mvc;
using MovieDashboard.Data;
using System.Linq;

namespace MovieDashboard.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ratings")]
        public IActionResult GetRatings()
        {
            var data = _context.FactMovies
                .Select(m => new
                {
                    m.Movie.Title,
                    m.Rating
                })
                .ToList();

            var result = new
            {
                labels = data.Select(d => d.Title).ToArray(),
                ratings = data.Select(d => d.Rating).ToArray()
            };

            return Ok(result);
        }
    }
}
