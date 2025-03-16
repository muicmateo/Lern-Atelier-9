using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VideoGameTracker.Views
{
    public partial class GameDetailView : UserControl
    {
        public GameDetailView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}