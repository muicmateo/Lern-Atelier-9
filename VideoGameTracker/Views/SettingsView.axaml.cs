using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VideoGameTracker.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}