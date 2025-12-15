using T4sV1.Views;

namespace T4sV1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register the route
            Routing.RegisterRoute("login", typeof(LoginPage));
           
            //// Optional but ensures the route is globally known
            //Routing.RegisterRoute("dashboard", typeof(T4sV1.Views.DashboardPage));
            Routing.RegisterRoute("home", typeof(T4sV1.Views.Homepage));
            //Routing.RegisterRoute("healthscore", typeof(HealthScorePage));
            //Routing.RegisterRoute("measurements", typeof(WeeklyMeasurementsPage));

            Routing.RegisterRoute("ParentDashboard", typeof(ParentDashboardPage));
            Routing.RegisterRoute("Profile", typeof(ProfilePage));         
            Routing.RegisterRoute("HealthScore", typeof(HealthScorePage));
            Routing.RegisterRoute("Measurement", typeof(MeasurementPage));

        }

    }
}
