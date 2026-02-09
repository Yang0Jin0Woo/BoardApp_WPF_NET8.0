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

        protected override void OnExit(ExitEventArgs e)
        {
            if (_services is IDisposable d) d.Dispose();
            base.OnExit(e);
        }

        private static IServiceProvider BuildServices()
        {
            var services = new ServiceCollection();

            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlite($"Data Source={AppDbContext.GetDbPath()}"));

            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<MainViewModel>();
            services.AddScoped<MainWindow>();

            return services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateScopes = true,
                ValidateOnBuild = true
            });
        }

        // Composition Root (애플리케이션 구성 전담)
        private static MainWindow CreateMainWindow(IServiceProvider services)
        {
            var scope = services.CreateScope();
            try
            {
                var scopedProvider = scope.ServiceProvider;
                var window = scopedProvider.GetRequiredService<MainWindow>();
                window.Closed += (_, __) => scope.Dispose();
                return window;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
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
