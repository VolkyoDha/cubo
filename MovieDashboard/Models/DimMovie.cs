using System.Collections.Generic;

namespace MovieDashboard.Models
{
    public class DimMovie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImdbLink { get; set; }
        public ICollection<FactMovie> FactMovies { get; set; }
    }
}
