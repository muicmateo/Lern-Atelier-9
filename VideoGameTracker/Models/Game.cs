using System;

namespace VideoGameTracker.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Platform { get; set; }
        public GameStatus Status { get; set; }
        public double Rating { get; set; }
        public string CoverImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime? LastPlayed { get; set; }
        public int? PlayTime { get; set; } // in minutes
    }
}