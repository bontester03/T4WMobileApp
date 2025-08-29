

using Microsoft.Extensions.DependencyInjection;
using T4sV1.Services;
using Syncfusion.Licensing;
using System.ComponentModel;


namespace T4sV1
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            ServiceProvider = serviceProvider;

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg3NDc3OEAzMjM5MmUzMDJlMzAzYjMyMzkzYlJobkJWRkpiZk4zZ1JNcmlOL2dKb2FVcDg4MFF4cjFQT3BDV3dyNkpXV2s9");


            var session = ServiceProvider.GetRequiredService<ISessionService>();
            session.LoadFromPreferences();

            MainPage = new LoginPage(session);
        }
    }
}
