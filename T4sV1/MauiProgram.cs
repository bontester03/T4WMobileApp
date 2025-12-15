using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkiaSharp.Extended.UI.Controls;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Core.Hosting;
using T4sV1.Model.ViewModels;
using T4sV1.Services;
using T4sV1.Services.Http;
using T4sV1.Services.Security;
using T4sV1.Views;


namespace T4sV1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMarkup()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.ConfigureSyncfusionCore();

#if ANDROID
    // Remove Entry underline on Android
    T4sV1.Platforms.Android.EntryMapper.RemoveUnderline();

    // Base API URL (Platform-specific)
    var isEmulator = Android.OS.Build.Fingerprint.Contains("generic")
                     || Android.OS.Build.Fingerprint.Contains("vbox")
                     || Android.OS.Build.Fingerprint.Contains("sdk_gphone");
    var baseUri = isEmulator
        ? new Uri("http://10.0.2.2:5206/")              // Emulator → localhost
        : new Uri("http://192.168.1.164:5206/");        // Real device → dev PC
#elif WINDOWS
    var baseUri = new Uri("http://localhost:5206/");    // Windows debugging
#else
            var baseUri = new Uri("https://webapit4s20250704142604-a0cndxeeabcgbpdk.uksouth-01.azurewebsites.net/");
#endif

            // ... rest of your code (service registrations, etc.)

            // ===== Core Services =====
            builder.Services.AddSingleton<ISessionService, SessionService>();
            builder.Services.AddSingleton<IActiveChildStore, ActiveChildStore>();
            builder.Services.AddSingleton<ITokenStore, TokenStore>();
            builder.Services.AddSingleton<IDashboardService, DashboardService>();
            builder.Services.AddSingleton<IProfileService, ProfileService>();
            builder.Services.AddSingleton<IHealthScoreService, HealthScoreService>();
            builder.Services.AddSingleton<IMeasurementService, MeasurementService>();


            // ===== Auth Service (No BearerAuthHandler) =====
            builder.Services.AddHttpClient<IAuthService, AuthService>(c =>
            {
                c.BaseAddress = baseUri;
                c.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<IHealthScoreService, HealthScoreService>(c =>
            {
                c.BaseAddress = baseUri; // Use your existing baseUri variable
                c.Timeout = TimeSpan.FromSeconds(30);
            })
               .AddHttpMessageHandler<BearerAuthHandler>();

            builder.Services.AddHttpClient<IMeasurementService, MeasurementService>(c =>
            {
                c.BaseAddress = baseUri; // Use your existing baseUri variable
                c.Timeout = TimeSpan.FromSeconds(30);
            })
               .AddHttpMessageHandler<BearerAuthHandler>();


            // ===== Authenticated HTTP Client (With BearerAuthHandler) =====
            builder.Services.AddTransient<BearerAuthHandler>();
            builder.Services.AddHttpClient("authed", c =>
            {
                c.BaseAddress = baseUri;
                c.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<BearerAuthHandler>();

            // ===== Children Service =====
            builder.Services.AddTransient<IChildrenService, ChildrenService>();

            // ===== Dashboard Service ===== ✅ ADD THIS LINE!
            builder.Services.AddTransient<IDashboardService, DashboardService>();

            // ===== ViewModels =====
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SelectActiveChildViewModel>();
            builder.Services.AddTransient<ChildrenListViewModel>();
            builder.Services.AddTransient<HomePageViewModel>(); // ✅ ADD THIS LINE!
            builder.Services.AddTransient<ParentDashboardViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<HealthScoreViewModel>();
            builder.Services.AddTransient<MeasurementViewModel>();

            // ===== Pages =====
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<T4sV1.Views.Homepage>();
            builder.Services.AddTransient<T4sV1.Views.SelectActiveChildPage>();
            builder.Services.AddTransient<T4sV1.Views.EResourcePage>();
            builder.Services.AddTransient<ParentDashboardPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<HealthScorePage>();
            builder.Services.AddTransient<MeasurementPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}