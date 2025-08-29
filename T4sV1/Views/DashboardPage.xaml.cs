using T4sV1.Services;
using T4sV1.Model;
using T4sV1.Model.ViewModels;
using System.Collections.ObjectModel;


namespace T4sV1.Views
{

    [QueryProperty(nameof(UserId), "userId")]
    [QueryProperty(nameof(Email), "email")]
    public partial class DashboardPage : ContentPage
    {

        public string UserId { get; set; }
        public string Email { get; set; }

        private readonly DashboardService _dashboardService = new();
        public DashboardViewModel ViewModel { get; set; } = new();

        private readonly ISessionService _session;

        private readonly List<string> _videoUrls = new()
{
    "https://www.youtube.com/embed/WHcLb0h9FmI",
    "https://www.youtube.com/embed/acxBR2K6dhs",
    "https://www.youtube.com/embed/KUHWjclsHjo",
    "https://www.youtube.com/embed/nTrfGo3sIiU",
    "https://www.youtube.com/embed/LheFxCohPdc"
};

        public DashboardPage()
        {
            InitializeComponent();
            // Resolve service manually using App.ServiceProvider
            _session = App.ServiceProvider.GetRequiredService<ISessionService>();
            BindingContext = ViewModel;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine($"UserId: {UserId}, Email: {Email}");
            await LoadDashboardAsync();
            int dayIndex = DateTime.Now.Day % _videoUrls.Count;
            VideoOfTheDay.Source = _videoUrls[dayIndex];
            VideoOfTheDay.IsVisible = true;
        }

        private async Task LoadDashboardAsync()
        {
            try
            {
                var userIdStr = await SecureStorage.Default.GetAsync("UserId");
                if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                {
                    await DisplayAlert("Error", "User not logged in.", "OK");
                    await Shell.Current.GoToAsync("//login");
                    return;
                }

                var dashboard = await _dashboardService.GetDashboardOverviewAsync(userId);
                if (dashboard == null)
                {
                    await DisplayAlert("Error", "Failed to load dashboard data.", "OK");
                    return;
                }

                ViewModel.UserName = dashboard.Email;
                ViewModel.RegistrationDate = dashboard.RegistrationDate;

                // 🧮 Summary counts
                ViewModel.TotalChildren = dashboard.Children?.Count ?? 0;
                ViewModel.TotalHealthScores = dashboard.HealthScores?.Count ?? 0;
                ViewModel.TotalWeightLogs = dashboard.Measurements?.Count ?? 0;

                // 🔁 Full data collections
                ViewModel.Activities = new ObservableCollection<TimelineActivityDto>(dashboard.Activities);
                ViewModel.Notifications = new ObservableCollection<NotificationDto>(dashboard.Notifications);
                ViewModel.Children = new ObservableCollection<ChildDto>(dashboard.Children);
                ViewModel.HealthScores = new ObservableCollection<HealthScoreDto>(dashboard.HealthScores);
                ViewModel.Measurements = new ObservableCollection<MeasurementDto>(dashboard.Measurements);

                // 📌 Refresh binding (in case it's not auto-notified)
                BindingContext = null;
                BindingContext = ViewModel;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
            }
        }

        private async void OnHealthScoresTapped(object sender, EventArgs e)
        {

            await Shell.Current.GoToAsync("healthscore");


        }

        private async void OnMeasurementsTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("measurements");
        }


        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (!confirm) return;

            // Clear session data
            var session = App.ServiceProvider.GetRequiredService<ISessionService>();
            session.Clear();

            // Redirect to login page
            Application.Current.MainPage = new NavigationPage(new LoginPage(session));
        }



    }
}
