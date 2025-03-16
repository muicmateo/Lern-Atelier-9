using System;

namespace VideoGameTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DateJoined { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string PreferredPlatform { get; set; }
        public string FavoriteGenre { get; set; }
    }
}