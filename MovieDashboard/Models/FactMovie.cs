using System.Collections.Generic;

namespace MovieDashboard.Models
{
    public class FactMovie
    {
        public int Id { get; set; }
        public DimMovie Movie { get; set; }
        public DimDirector Director { get; set; }
        public DimGenre Genre { get; set; }
        public int Year { get; set; }
        public int Duration { get; set; }
        public float Rating { get; set; }
        public int RatingCount { get; set; }
    }
}
