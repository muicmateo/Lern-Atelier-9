using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VideoGameTracker.Views
{
    public partial class LibraryView : UserControl
    {
        public LibraryView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}