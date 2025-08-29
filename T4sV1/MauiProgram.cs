using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using T4sV1.Services;
using Syncfusion.Maui.Core.Hosting;
using CommunityToolkit.Maui.Markup;




namespace T4sV1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()                 
                .UseMauiCommunityToolkit() // ✅ This line is required
                 .UseMauiCommunityToolkitMarkup()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });



            builder.ConfigureSyncfusionCore();

            // Register the session service
            builder.Services.AddSingleton<ISessionService, SessionService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
