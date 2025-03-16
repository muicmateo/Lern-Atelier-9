using System.Collections.Generic;
using System.Threading.Tasks;
using VideoGameTracker.Models;

namespace VideoGameTracker.Services
{
    public interface IGameDataService
    {
        Task<IEnumerable<Game>> GetAllGamesAsync();
        Task<Game> GetGameByIdAsync(int id);
        Task<bool> AddGameAsync(Game game);
        Task<bool> UpdateGameAsync(Game game);
        Task<bool> DeleteGameAsync(int id);
        Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm);
        Task<IEnumerable<Game>> FilterGamesByStatusAsync(GameStatus status);
        Task<IEnumerable<Game>> FilterGamesByPlatformAsync(string platform);
        Task<IEnumerable<Game>> GetFavoriteGamesAsync();
    }
}