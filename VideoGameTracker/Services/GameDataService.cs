using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoGameTracker.Models;

namespace VideoGameTracker.Services
{
    public class GameDataService : IGameDataService
    {
        private List<Game> _games;

        public GameDataService()
        {
            // Mock data for demonstration
            _games = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Title = "The Legend of Zelda: Breath of the Wild",
                    Developer = "Nintendo",
                    Publisher = "Nintendo",
                    Year = 2017,
                    Genre = "Action-Adventure",
                    Platform = "Nintendo Switch",
                    Status = GameStatus.Completed,
                    Rating = 9.5,
                    CoverImageUrl = "https://example.com/botw.jpg",
                    Description = "An open-world action-adventure game featuring exploration, puzzle-solving, and combat.",
                    IsFavorite = true,
                    LastPlayed = DateTime.Now.AddDays(-15),
                    PlayTime = 12000 // 200 hours
                },
                new Game
                {
                    Id = 2,
                    Title = "Elden Ring",
                    Developer = "FromSoftware",
                    Publisher = "Bandai Namco",
                    Year = 2022,
                    Genre = "Action RPG",
                    Platform = "PC",
                    Status = GameStatus.InProgress,
                    Rating = 9.0,
                    CoverImageUrl = "https://example.com/eldenring.jpg",
                    Description = "An action RPG featuring an open world with a fantasy setting.",
                    IsFavorite = true,
                    LastPlayed = DateTime.Now.AddDays(-2),
                    PlayTime = 4800 // 80 hours
                },
                new Game
                {
                    Id = 3,
                    Title = "Cyberpunk 2077",
                    Developer = "CD Projekt Red",
                    Publisher = "CD Projekt",
                    Year = 2020,
                    Genre = "Action RPG",
                    Platform = "PC",
                    Status = GameStatus.Abandoned,
                    Rating = 7.0,
                    CoverImageUrl = "https://example.com/cyberpunk.jpg",
                    Description = "An open-world, action-adventure RPG set in a dystopian future.",
                    IsFavorite = false,
                    LastPlayed = DateTime.Now.AddMonths(-6),
                    PlayTime = 1800 // 30 hours
                }
            };
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            // Simulate async operation
            await Task.Delay(100);
            return _games;
        }

        public async Task<Game> GetGameByIdAsync(int id)
        {
            await Task.Delay(50);
            return _games.FirstOrDefault(g => g.Id == id);
        }

        public async Task<bool> AddGameAsync(Game game)
        {
            await Task.Delay(100);

            // Assign a new ID (in a real application, this would be handled by the database)
            game.Id = _games.Count > 0 ? _games.Max(g => g.Id) + 1 : 1;

            _games.Add(game);
            return true;
        }

        public async Task<bool> UpdateGameAsync(Game game)
        {
            await Task.Delay(100);

            var existingGame = _games.FirstOrDefault(g => g.Id == game.Id);
            if (existingGame == null)
                return false;

            // Update the game properties
            var index = _games.IndexOf(existingGame);
            _games[index] = game;

            return true;
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            await Task.Delay(50);

            var game = _games.FirstOrDefault(g => g.Id == id);
            if (game == null)
                return false;

            _games.Remove(game);
            return true;
        }

        public async Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm)
        {
            await Task.Delay(50);

            if (string.IsNullOrWhiteSpace(searchTerm))
                return _games;

            searchTerm = searchTerm.ToLower();

            return _games.Where(g =>
                g.Title.ToLower().Contains(searchTerm) ||
                g.Developer.ToLower().Contains(searchTerm) ||
                g.Publisher.ToLower().Contains(searchTerm) ||
                g.Genre.ToLower().Contains(searchTerm) ||
                g.Description.ToLower().Contains(searchTerm)
            );
        }

        public async Task<IEnumerable<Game>> FilterGamesByStatusAsync(GameStatus status)
        {
            await Task.Delay(50);
            return _games.Where(g => g.Status == status);
        }

        public async Task<IEnumerable<Game>> FilterGamesByPlatformAsync(string platform)
        {
            await Task.Delay(50);

            if (string.IsNullOrWhiteSpace(platform))
                return _games;

            platform = platform.ToLower();

            return _games.Where(g => g.Platform.ToLower().Contains(platform));
        }

        public async Task<IEnumerable<Game>> GetFavoriteGamesAsync()
        {
            await Task.Delay(50);
            return _games.Where(g => g.IsFavorite);
        }
    }
}