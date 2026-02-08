using System.Windows;

namespace BoardApp.Views
{
    public partial class MainWindow : Window
    {
        private bool _loadedOnce;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (_, __) =>
            {
                if (_loadedOnce) return;
                _loadedOnce = true;

                if (DataContext is BoardApp.ViewModels.MainViewModel vm)
                    await vm.LoadAsync();
            };
        }
    }
}
