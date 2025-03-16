using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using VideoGameTracker.Models;
using VideoGameTracker.Services;

namespace VideoGameTracker.ViewModels
{
    public class LibraryViewModel : ViewModelBase
    {
        private readonly IGameDataService _gameDataService;
        private ObservableCollection<GameViewModel> _games;
        private GameViewModel _selectedGame;
        private string _searchText;
        private bool _isLoading;
        private string _statusFilter;
        private string _platformFilter;

        public LibraryViewModel(IGameDataService gameDataService)
        {
            _gameDataService = gameDataService;

            // Initialize commands
            SearchCommand = ReactiveCommand.CreateFromTask(SearchGamesAsync);
            AddGameCommand = ReactiveCommand.Create(AddNewGame);
            EditGameCommand = ReactiveCommand.Create<GameViewModel>(EditGame);
            DeleteGameCommand = ReactiveCommand.CreateFromTask<GameViewModel>(DeleteGameAsync);
            RefreshCommand = ReactiveCommand.CreateFromTask(LoadGamesAsync);
            FilterByStatusCommand = ReactiveCommand.CreateFromTask<string>(FilterByStatusAsync);
            FilterByPlatformCommand = ReactiveCommand.CreateFromTask<string>(FilterByPlatformAsync);

            // Status filter options
            StatusFilterOptions = new ObservableCollection<string>
            {
                "All",
                "Not Started",
                "In Progress",
                "Completed",
                "Abandoned",
                "On Hold",
                "Wishlist"
            };

            // Platform filter options (would be populated dynamically in a real app)
            PlatformFilterOptions = new ObservableCollection<string>
            {
                "All",
                "PC",
                "PlayStation 5",
                "PlayStation 4",
                "Xbox Series X/S",
                "Xbox One",
                "Nintendo Switch"
            };

            // Load games
            Task.Run(LoadGamesAsync);
        }

        public ObservableCollection<GameViewModel> Games
        {
            get => _games;
            set => SetProperty(ref _games, value);
        }

        public GameViewModel SelectedGame
        {
            get => _selectedGame;
            set => SetProperty(ref _selectedGame, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string StatusFilter
        {
            get => _statusFilter;
            set
            {
                if (SetProperty(ref _statusFilter, value))
                {
                    // Apply filter when the status changes
                    Task.Run(() => FilterByStatusAsync(value));
                }
            }
        }

        public string PlatformFilter
        {
            get => _platformFilter;
            set
            {
                if (SetProperty(ref _platformFilter, value))
                {
                    // Apply filter when the platform changes
                    Task.Run(() => FilterByPlatformAsync(value));
                }
            }
        }

        public ObservableCollection<string> StatusFilterOptions { get; }
        public ObservableCollection<string> PlatformFilterOptions { get; }

        // Commands
        public ICommand SearchCommand { get; }
        public ICommand AddGameCommand { get; }
        public ICommand EditGameCommand { get; }
        public ICommand DeleteGameCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand FilterByStatusCommand { get; }
        public ICommand FilterByPlatformCommand { get; }

        private async Task LoadGamesAsync()
        {
            IsLoading = true;

            try
            {
                var games = await _gameDataService.GetAllGamesAsync();

                // Convert Model to ViewModel
                Games = new ObservableCollection<GameViewModel>(
                    games.Select(g => new GameViewModel(g, _gameDataService))
                );
            }
            catch (Exception ex)
            {
                // In a real app, you'd have proper error handling
                Console.WriteLine($"Error loading games: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchGamesAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadGamesAsync();
                return;
            }

            IsLoading = true;

            try
            {
                var games = await _gameDataService.SearchGamesAsync(SearchText);

                // Convert Model to ViewModel
                Games = new ObservableCollection<GameViewModel>(
                    games.Select(g => new GameViewModel(g, _gameDataService))
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching games: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddNewGame()
        {
            // In a real app, you'd navigate to a new game form or show a dialog
            var newGame = new Game
            {
                Title = "New Game",
                Status = GameStatus.NotStarted,
                Year = DateTime.Now.Year
            };

            // Add to service and refresh
            _gameDataService.AddGameAsync(newGame);
            Task.Run(LoadGamesAsync);
        }

        private void EditGame(GameViewModel game)
        {
            // In a real app, you'd navigate to the edit form or show a dialog
            SelectedGame = game;
        }

        private async Task DeleteGameAsync(GameViewModel game)
        {
            // In a real app, you'd show a confirmation dialog
            if (game != null)
            {
                await _gameDataService.DeleteGameAsync(game.Id);
                await LoadGamesAsync();
            }
        }

        private async Task FilterByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status) || status == "All")
            {
                await LoadGamesAsync();
                return;
            }

            IsLoading = true;

            try
            {
                // Convert string status to enum
                if (Enum.TryParse(status.Replace(" ", ""), out GameStatus gameStatus))
                {
                    var games = await _gameDataService.FilterGamesByStatusAsync(gameStatus);

                    // Convert Model to ViewModel
                    Games = new ObservableCollection<GameViewModel>(
                        games.Select(g => new GameViewModel(g, _gameDataService))
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering games by status: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task FilterByPlatformAsync(string platform)
        {
            if (string.IsNullOrWhiteSpace(platform) || platform == "All")
            {
                await LoadGamesAsync();
                return;
            }

            IsLoading = true;

            try
            {
                var games = await _gameDataService.FilterGamesByPlatformAsync(platform);

                // Convert Model to ViewModel
                Games = new ObservableCollection<GameViewModel>(
                    games.Select(g => new GameViewModel(g, _gameDataService))
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering games by platform: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}