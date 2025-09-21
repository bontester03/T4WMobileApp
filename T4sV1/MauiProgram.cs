using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using T4sV1.Model.Auth; // LoginRequestDto, LoginResponseDto
using T4sV1.Services;            // IAuthService, AuthService
using T4sV1.Services.Http;       // BearerAuthHandler
using T4sV1.Services.Security;   // ITokenStore, TokenStore
using T4sV1.Model.ViewModels;          // LoginViewModel
using Microsoft.Extensions.DependencyInjection; // for AddHttpClient extensions


namespace T4sV1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()          // ✅ keep
                .UseMauiCommunityToolkitMarkup()    // ✅ keep
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.ConfigureSyncfusionCore();       // ✅ keep

            // ===== Base API URL (DEV over HTTP on port 5206) =====
            // MauiProgram.cs
#if ANDROID
    var isEmulator = Android.OS.Build.Fingerprint.Contains("generic")
                     || Android.OS.Build.Fingerprint.Contains("vbox")
                     || Android.OS.Build.Fingerprint.Contains("sdk_gphone");

    // 🟢 Choose target based on your need
    var baseUri = isEmulator
        ? new Uri("http://10.0.2.2:5206/")              // Emulator → localhost
        : new Uri("http://192.168.1.164:5206/");        // Real device → dev PC over Wi-Fi
#elif WINDOWS
    var baseUri = new Uri("http://localhost:5206/");    // Windows debugging
#else
            var baseUri = new Uri("https://webapit4s20250704142604-a0cndxeeabcgbpdk.uksouth-01.azurewebsites.net/");
#endif



            // ===== App Services =====
            builder.Services.AddSingleton<IActiveChildStore, ActiveChildStore>();

            // ViewModel + Page
            builder.Services.AddTransient<T4sV1.Model.ViewModels.SelectActiveChildViewModel>();
            builder.Services.AddTransient<T4sV1.Views.SelectActiveChildPage>();

            // (You already have IChildrenService, IAuthService, ISessionService, BearerAuthHandler, etc.)

            // ✅ Children in Security namespace
            builder.Services.AddTransient<T4sV1.Services.Security.IChildrenService,
                                          T4sV1.Services.ChildrenService>();

            // ===== Auth wiring =====
            // Secure token storage (SecureStorage + Preferences)
            builder.Services.AddSingleton<ITokenStore, TokenStore>();

            // Auth client (used only by AuthService for login/refresh/logout)
            builder.Services.AddHttpClient<IAuthService, AuthService>(c =>
            {
                c.BaseAddress = baseUri;
                c.Timeout = TimeSpan.FromSeconds(30);
            });

            // General API client with auto Bearer + auto refresh on 401
            builder.Services.AddTransient<BearerAuthHandler>();
            builder.Services.AddHttpClient("authed", c =>
            {
                c.BaseAddress = baseUri;
                c.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<BearerAuthHandler>();

            // Pages
            builder.Services.AddTransient<T4sV1.Views.HomePage>();
            // ViewModels (bind your existing Login page to this VM)
            builder.Services.AddTransient<LoginViewModel>();

            //login page
            builder.Services.AddTransient<LoginPage>();

            // Your existing services (keep)
            builder.Services.AddSingleton<ISessionService, SessionService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
