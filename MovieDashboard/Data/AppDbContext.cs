using Microsoft.EntityFrameworkCore;
using MovieDashboard.Models;

namespace MovieDashboard.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<FactMovie> FactMovies { get; set; }
        public DbSet<DimMovie> DimMovies { get; set; }
        public DbSet<DimDirector> DimDirectors { get; set; }
        public DbSet<DimGenre> DimGenres { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
