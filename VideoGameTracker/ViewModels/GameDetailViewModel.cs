using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using VideoGameTracker.Models;
using VideoGameTracker.Services;

namespace VideoGameTracker.ViewModels
{
    public class GameDetailViewModel : ViewModelBase
    {
        private readonly IGameDataService _gameDataService;
        private Game _game;
        private bool _isEditing;
        private string _title;
        private string _developer;
        private string _publisher;
        private int _year;
        private string _genre;
        private string _platform;
        private GameStatus _status;
        private double _rating;
        private string _coverImageUrl;
        private string _description;
        private bool _isFavorite;

        public GameDetailViewModel(IGameDataService gameDataService, int gameId = 0)
        {
            _gameDataService = gameDataService;

            // Commands
            SaveCommand = ReactiveCommand.CreateFromTask(SaveGameAsync);
            CancelCommand = ReactiveCommand.Create(CancelEditing);
            StartEditingCommand = ReactiveCommand.Create(StartEditing);

            // Initialize status options
            StatusOptions = new ObservableCollection<GameStatus>
            {
                GameStatus.NotStarted,
                GameStatus.InProgress,
                GameStatus.Completed,
                GameStatus.Abandoned,
                GameStatus.OnHold,
                GameStatus.Wishlist
            };

            // If gameId is provided, load the game
            if (gameId > 0)
            {
                Task.Run(() => LoadGameAsync(gameId));
            }
            else
            {
                // New game
                _game = new Game
                {
                    Title = "New Game",
                    Year = DateTime.Now.Year,
                    Status = GameStatus.NotStarted
                };
                IsEditing = true;
                UpdateProperties();
            }
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Developer
        {
            get => _developer;
            set => SetProperty(ref _developer, value);
        }

        public string Publisher
        {
            get => _publisher;
            set => SetProperty(ref _publisher, value);
        }

        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        public string Genre
        {
            get => _genre;
            set => SetProperty(ref _genre, value);
        }

        public string Platform
        {
            get => _platform;
            set => SetProperty(ref _platform, value);
        }

        public GameStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public double Rating
        {
            get => _rating;
            set => SetProperty(ref _rating, value);
        }

        public string CoverImageUrl
        {
            get => _coverImageUrl;
            set => SetProperty(ref _coverImageUrl, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }

        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        public ObservableCollection<GameStatus> StatusOptions { get; }

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand StartEditingCommand { get; }

        // Additional display-only properties
        public string FormattedPlayTime
        {
            get
            {
                if (_game?.PlayTime == null || _game.PlayTime <= 0)
                    return "Not played";

                var hours = _game.PlayTime.Value / 60;
                var minutes = _game.PlayTime.Value % 60;

                if (hours > 0)
                    return $"{hours}h {minutes}m";
                else
                    return $"{minutes}m";
            }
        }

        public string LastPlayedText
        {
            get
            {
                if (_game?.LastPlayed == null)
                    return "Never played";

                var daysAgo = (DateTime.Now - _game.LastPlayed.Value).Days;

                if (daysAgo == 0)
                    return "Today";
                else if (daysAgo == 1)
                    return "Yesterday";
                else
                    return $"{daysAgo} days ago";
            }
        }

        private async Task LoadGameAsync(int gameId)
        {
            try
            {
                _game = await _gameDataService.GetGameByIdAsync(gameId);
                if (_game != null)
                {
                    UpdateProperties();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game details: {ex.Message}");
            }
        }

        private void UpdateProperties()
        {
            // Update the view properties from the model
            Title = _game.Title;
            Developer = _game.Developer;
            Publisher = _game.Publisher;
            Year = _game.Year;
            Genre = _game.Genre;
            Platform = _game.Platform;
            Status = _game.Status;
            Rating = _game.Rating;
            CoverImageUrl = _game.CoverImageUrl;
            Description = _game.Description;
            IsFavorite = _game.IsFavorite;

            // Update the display-only properties
            OnPropertyChanged(nameof(FormattedPlayTime));
            OnPropertyChanged(nameof(LastPlayedText));
        }

        private void StartEditing()
        {
            IsEditing = true;
        }

        private void CancelEditing()
        {
            if (_game.Id == 0)
            {
                // This was a new game
                // In a real app, you'd probably navigate back or close the dialog
            }
            else
            {
                // Revert changes
                UpdateProperties();
                IsEditing = false;
            }
        }

        private async Task SaveGameAsync()
        {
            // Update the model from the view properties
            _game.Title = Title;
            _game.Developer = Developer;
            _game.Publisher = Publisher;
            _game.Year = Year;
            _game.Genre = Genre;
            _game.Platform = Platform;
            _game.Status = Status;
            _game.Rating = Rating;
            _game.CoverImageUrl = CoverImageUrl;
            _game.Description = Description;
            _game.IsFavorite = IsFavorite;

            try
            {
                if (_game.Id == 0)
                {
                    // New game
                    await _gameDataService.AddGameAsync(_game);
                }
                else
                {
                    // Existing game
                    await _gameDataService.UpdateGameAsync(_game);
                }

                IsEditing = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game: {ex.Message}");
            }
        }
    }
}