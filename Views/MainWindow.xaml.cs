using BoardApp.ViewModels;
using System.Windows;

namespace BoardApp.Views
{
    public partial class MainWindow : Window
    {
        private bool _initialized;
        private readonly MainViewModel _vm;
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        protected override async void OnContentRendered(System.EventArgs e)
        {
            base.OnContentRendered(e);
            if (_initialized) return;
            _initialized = true;

            await _vm.LoadAsync();
        }
    }
}
