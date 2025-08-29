using T4sV1.Model;
using System.Collections.ObjectModel;
using T4sV1.Services;

namespace T4sV1.Views;

public partial class HealthScorePage : ContentPage
{
    public ObservableCollection<HealthScoreDto> HealthScores { get; set; } = new();
    private readonly DashboardService _dashboardService = new();
    private readonly ISessionService _session;

    public HealthScorePage()
    {
        InitializeComponent();
        _session = App.ServiceProvider.GetRequiredService<ISessionService>();
        _session.LoadFromPreferences();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!int.TryParse(_session.UserId, out int userId))
        {
            await DisplayAlert("Error", "User not logged in", "OK");
            await Shell.Current.GoToAsync("//login");
            return;
        }

        var dashboard = await _dashboardService.GetDashboardOverviewAsync(userId);
        if (dashboard?.HealthScores != null)
        {
            HealthScores.Clear();
            foreach (var score in dashboard.HealthScores)
                HealthScores.Add(score);
        }
    }
}
