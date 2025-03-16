using System;

namespace VideoGameTracker.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public double Score { get; set; } // 1.0 to 10.0
        public string Comment { get; set; }
        public DateTime DateRated { get; set; }
    }
}