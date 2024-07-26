using System.Collections.Generic;

namespace MovieDashboard.Models
{
    public class DimGenre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<FactMovie> FactMovies { get; set; }
    }
}
