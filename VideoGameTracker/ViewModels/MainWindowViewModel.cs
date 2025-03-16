using System;
using System.Windows.Input;
using ReactiveUI;
using VideoGameTracker.Services;

namespace VideoGameTracker.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;
        private readonly IGameDataService _gameDataService;
        private readonly IGithubService _githubService;

        public MainWindowViewModel(IGameDataService gameDataService, IGithubService githubService)
        {
            _gameDataService = gameDataService;
            _githubService = githubService;

            // Initialize with the library view
            LibraryViewModel = new LibraryViewModel(_gameDataService);
            CurrentView = LibraryViewModel;

            // Commands
            NavigateLibraryCommand = ReactiveCommand.Create(NavigateToLibrary);
            NavigateSettingsCommand = ReactiveCommand.Create(NavigateToSettings);
        }

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        // Sub-viewmodels
        public LibraryViewModel LibraryViewModel { get; }
        public SettingsViewModel SettingsViewModel => new SettingsViewModel(_githubService);

        // Navigation commands
        public ICommand NavigateLibraryCommand { get; }
        public ICommand NavigateSettingsCommand { get; }

        private void NavigateToLibrary()
        {
            CurrentView = LibraryViewModel;
        }

        private void NavigateToSettings()
        {
            CurrentView = SettingsViewModel;
        }
    }
}