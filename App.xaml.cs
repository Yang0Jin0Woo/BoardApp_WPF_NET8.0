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

            if (!TryMigrateDatabase())
            {
                Shutdown(-1);
                return;
            }

            var window = CreateMainWindow();
            window.Show();
        }

        // Composition Root (애플리케이션 구성 전담)
        private MainWindow CreateMainWindow()
        {
            var vm = CreateMainViewModel();
            return new MainWindow { DataContext = vm };
        }

        private MainViewModel CreateMainViewModel()
        {
            var service = CreatePostService();
            return new MainViewModel(service);
        }

        private IPostService CreatePostService()
        {
            var repo = CreatePostRepository();
            return new PostService(repo);
        }

        private IPostRepository CreatePostRepository()
        {
            var dbFactory = CreateDbFactory();
            return new PostRepository(dbFactory);
        }

        private static Func<AppDbContext> CreateDbFactory()
            => () => new AppDbContext();

        // Startup (인프라 초기화)
        private static bool TryMigrateDatabase()
        {
            try
            {
                using var db = new AppDbContext();
                db.Database.Migrate();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"DB 초기화(마이그레이션)에 실패했습니다.\n\n{ex.Message}",
                    "Startup Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }
        }
    }
}
