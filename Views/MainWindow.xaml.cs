using System.Windows;

namespace BoardApp.Views
{
    public partial class MainWindow : Window
    {
        private bool _initialized;
        public MainWindow(BoardApp.ViewModels.MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        protected override async void OnContentRendered(System.EventArgs e)
        {
            base.OnContentRendered(e);
            if (_initialized) return;
            _initialized = true;

            if (DataContext is BoardApp.ViewModels.MainViewModel vm)
                await vm.LoadAsync();
        }
    }
}
