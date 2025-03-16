using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using VideoGameTracker.Models;
using VideoGameTracker.Services;

namespace VideoGameTracker.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private readonly IGameDataService _gameDataService;
        private Game _game;

        public GameViewModel(Game game, IGameDataService gameDataService)
        {
            _game = game;
            _gameDataService = gameDataService;

            // Initialize commands
            ToggleFavoriteCommand = ReactiveCommand.CreateFromTask(ToggleFavoriteAsync);
            UpdateStatusCommand = ReactiveCommand.CreateFromTask<GameStatus>(UpdateGameStatusAsync);
        }

        // Properties mapped from the Game model
        public int Id => _game.Id;
        public string Title => _game.Title;
        public string Developer => _game.Developer;
        public string Publisher => _game.Publisher;
        public int Year => _game.Year;
        public string Genre => _game.Genre;
        public string Platform => _game.Platform;
        public GameStatus Status => _game.Status;
        public double Rating => _game.Rating;
        public string CoverImageUrl => _game.CoverImageUrl;
        public string Description => _game.Description;
        public bool IsFavorite => _game.IsFavorite;
        public DateTime? LastPlayed => _game.LastPlayed;
        public int? PlayTime => _game.PlayTime;

        // Additional view-related properties
        public string FormattedPlayTime
        {
            get
            {
                if (!PlayTime.HasValue || PlayTime.Value <= 0)
                    return "Not played";

                var hours = PlayTime.Value / 60;
                var minutes = PlayTime.Value % 60;

                if (hours > 0)
                    return $"{hours}h {minutes}m";
                else
                    return $"{minutes}m";
            }
        }

        public string StatusText => Status.ToString();

        public string LastPlayedText
        {
            get
            {
                if (!LastPlayed.HasValue)
                    return "Never played";

                var daysAgo = (DateTime.Now - LastPlayed.Value).Days;

                if (daysAgo == 0)
                    return "Today";
                else if (daysAgo == 1)
                    return "Yesterday";
                else
                    return $"{daysAgo} days ago";
            }
        }

        // Commands
        public ICommand ToggleFavoriteCommand { get; }
        public ICommand UpdateStatusCommand { get; }

        private async Task ToggleFavoriteAsync()
        {
            // Update the model
            _game.IsFavorite = !_game.IsFavorite;

            // Save changes
            await _gameDataService.UpdateGameAsync(_game);

            // Notify UI
            OnPropertyChanged(nameof(IsFavorite));
        }

        private async Task UpdateGameStatusAsync(GameStatus newStatus)
        {
            // Update the model
            _game.Status = newStatus;

            // If status changed to InProgress, update LastPlayed
            if (newStatus == GameStatus.InProgress)
            {
                _game.LastPlayed = DateTime.Now;
            }

            // Save changes
            await _gameDataService.UpdateGameAsync(_game);

            // Notify UI
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(StatusText));
            OnPropertyChanged(nameof(LastPlayed));
            OnPropertyChanged(nameof(LastPlayedText));
        }
    }
}