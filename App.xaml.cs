using BoardApp.Data;
using BoardApp.Repositories;
using BoardApp.Services;
using BoardApp.ViewModels;
using BoardApp.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace BoardApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                using var db = new AppDbContext();
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"DB 초기화(마이그레이션)에 실패했습니다.\n\n{ex.Message}",
                    "Startup Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Shutdown(-1);
                return;
            }

            Func<AppDbContext> dbFactory = () => new AppDbContext();

            IPostRepository repo = new PostRepository(dbFactory);
            IPostService service = new PostService(repo);
            var vm = new MainViewModel(service);

            var window = new MainWindow { DataContext = vm };
            window.Show();
        }
    }
}
