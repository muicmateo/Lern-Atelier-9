using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using VideoGameTracker.Services;
using VideoGameTracker.ViewModels;
using VideoGameTracker.Views;

namespace VideoGameTracker
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // Setup dependency injection
            var services = new ServiceCollection();

            // Register services
            services.AddSingleton<IGameDataService, GameDataService>();
            services.AddSingleton<IGithubService, GitHubService>();

            // Register viewmodels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<LibraryViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<GameDetailViewModel>();

            Services = services.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindowViewModel = Services.GetRequiredService<MainWindowViewModel>();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainWindowViewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}