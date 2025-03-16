using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using VideoGameTracker.Services;

namespace VideoGameTracker.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IGithubService _githubService;
        private string _currentVersion;
        private string _latestVersion;
        private bool _updateAvailable;
        private string _releaseNotes;
        private bool _isCheckingForUpdates;
        private string _selectedTheme;
        private bool _enableNotifications;
        private bool _autoCheckForUpdates;

        public SettingsViewModel(IGithubService githubService)
        {
            _githubService = githubService;

            // Default values
            _currentVersion = "1.0.0";
            _selectedTheme = "System Default";
            _enableNotifications = true;
            _autoCheckForUpdates = true;

            // Theme options
            ThemeOptions = new ObservableCollection<string>
            {
                "System Default",
                "Light",
                "Dark"
            };

            // Commands
            CheckForUpdatesCommand = ReactiveCommand.CreateFromTask(CheckForUpdatesAsync);
            SaveSettingsCommand = ReactiveCommand.Create(SaveSettings);
            ResetSettingsCommand = ReactiveCommand.Create(ResetSettings);

            // Check for updates on startup
            if (_autoCheckForUpdates)
            {
                Task.Run(CheckForUpdatesAsync);
            }
        }

        public string CurrentVersion
        {
            get => _currentVersion;
            set => SetProperty(ref _currentVersion, value);
        }

        public string LatestVersion
        {
            get => _latestVersion;
            set => SetProperty(ref _latestVersion, value);
        }

        public bool UpdateAvailable
        {
            get => _updateAvailable;
            set => SetProperty(ref _updateAvailable, value);
        }

        public string ReleaseNotes
        {
            get => _releaseNotes;
            set => SetProperty(ref _releaseNotes, value);
        }

        public bool IsCheckingForUpdates
        {
            get => _isCheckingForUpdates;
            set => SetProperty(ref _isCheckingForUpdates, value);
        }

        public string SelectedTheme
        {
            get => _selectedTheme;
            set => SetProperty(ref _selectedTheme, value);
        }

        public bool EnableNotifications
        {
            get => _enableNotifications;
            set => SetProperty(ref _enableNotifications, value);
        }

        public bool AutoCheckForUpdates
        {
            get => _autoCheckForUpdates;
            set => SetProperty(ref _autoCheckForUpdates, value);
        }

        public ObservableCollection<string> ThemeOptions { get; }

        // Commands
        public ICommand CheckForUpdatesCommand { get; }
        public ICommand SaveSettingsCommand { get; }
        public ICommand ResetSettingsCommand { get; }

        private async Task CheckForUpdatesAsync()
        {
            IsCheckingForUpdates = true;

            try
            {
                // Get the latest version from GitHub
                LatestVersion = await _githubService.GetLatestVersionAsync();

                // Check if an update is available
                UpdateAvailable = await _githubService.CheckForUpdatesAsync();

                if (UpdateAvailable)
                {
                    // Get release notes
                    ReleaseNotes = await _githubService.GetReleaseNotesAsync(LatestVersion);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking for updates: {ex.Message}");
            }
            finally
            {
                IsCheckingForUpdates = false;
            }
        }

        private void SaveSettings()
        {
            // In a real app, you would save the settings to a configuration file or database
            Console.WriteLine("Settings saved");
        }

        private void ResetSettings()
        {
            // Reset to default values
            SelectedTheme = "System Default";
            EnableNotifications = true;
            AutoCheckForUpdates = true;

            // In a real app, you would save the reset settings
            SaveSettings();
        }
    }
}