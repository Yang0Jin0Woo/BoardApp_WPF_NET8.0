using BoardApp.Data;
using BoardApp.Repositories;
using BoardApp.Services;
using BoardApp.ViewModels;
using BoardApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace BoardApp
{
    public partial class App : Application
    {
        private IServiceProvider? _services;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _services = BuildServices();

            if (!TryMigrateDatabase(_services))
            {
                Shutdown(-1);
                return;
            }

            var window = CreateMainWindow(_services);
            window.Show();
        }

        private static IServiceProvider BuildServices()
        {
            var services = new ServiceCollection();

            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlite($"Data Source={AppDbContext.GetDbPath()}"));

            services.AddSingleton<IPostRepository, PostRepository>();
            services.AddSingleton<IPostService, PostService>();
            services.AddTransient<MainViewModel>();
            services.AddSingleton<MainWindow>();

            return services.BuildServiceProvider();
        }

        // Composition Root (애플리케이션 구성 전담)
        private static MainWindow CreateMainWindow(IServiceProvider services)
        {
            var window = services.GetRequiredService<MainWindow>();
            window.DataContext = services.GetRequiredService<MainViewModel>();
            return window;
        }

        // Startup (인프라 초기화)
        private static bool TryMigrateDatabase(IServiceProvider services)
        {
            try
            {
                var factory = services.GetRequiredService<IDbContextFactory<AppDbContext>>();
                using var db = factory.CreateDbContext();
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
