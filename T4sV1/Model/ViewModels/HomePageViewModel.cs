using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using T4sV1.Model;
using T4sV1.Model.Children;
using T4sV1.Model.Dashboard;
using T4sV1.Services;
using T4sV1.Services.Http;
using T4sV1.Services.Security;

namespace T4sV1.Model.ViewModels;

public sealed class HomePageViewModel : INotifyPropertyChanged
{
    private readonly IDashboardService _dashboardService;
    private readonly IActiveChildStore _activeChildStore;
    private readonly IAuthService _auth;
    private readonly ISessionService _session;

    private DashboardResponse? _dashboardData;
    private bool _isLoading;
    private bool _hasChild;
    private string _errorMessage = "";
    public ICommand GoToProfileCommand { get; }
    public HomePageViewModel(
        IDashboardService dashboardService,
        IActiveChildStore activeChildStore,
        IAuthService auth,
        ISessionService session)
    {
        _dashboardService = dashboardService;
        _activeChildStore = activeChildStore;
        _auth = auth;
        _session = session;

        LogoutCommand = new Command(async () => await LogoutAsync());
        ChangeChildCommand = new Command(async () => await ChangeChildAsync());
        RefreshCommand = new Command(async () => await LoadDashboardAsync());
        GoToProfileCommand = new Command(async () => await GoToProfileAsync()); // ✅ Add this
    }

    // Properties
    private async Task GoToProfileAsync()
    {
        if (!HasChild)
        {
            await Application.Current.MainPage.DisplayAlert("No Child Selected", "Please select a child first", "OK");
            return;
        }

        try
        {
            // Get the active child ID (should already be set from LoadDashboardAsync)
            var activeChildId = await _activeChildStore.GetAsync();

            System.Diagnostics.Debug.WriteLine($"🎯 Going to Profile from HomePage");
            System.Diagnostics.Debug.WriteLine($"   - Active Child ID in Store: {activeChildId}");
            System.Diagnostics.Debug.WriteLine($"   - Session Active Child ID: {_session.ActiveChildId}");

            // Double-check it's set correctly
            if (!activeChildId.HasValue && _dashboardData?.ActiveChildId.HasValue == true)
            {
                // Fallback: Re-set from dashboard data
                await _activeChildStore.SetAsync(_dashboardData.ActiveChildId.Value);
                _session.ActiveChildId = _dashboardData.ActiveChildId.Value;
                _session.SaveToPreferences();

                System.Diagnostics.Debug.WriteLine($"   - Re-set active child to: {_dashboardData.ActiveChildId.Value}");
            }

            // Navigate to Profile
            await Shell.Current.GoToAsync("Profile");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to open profile: {ex.Message}", "OK");
        }
    }
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public bool HasChild
    {
        get => _hasChild;
        set { _hasChild = value; OnPropertyChanged(); OnPropertyChanged(nameof(NoChildSelected)); }
    }

    public bool NoChildSelected => !HasChild;

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public string ChildName { get; set; } = "No child selected";
    public string ChildDetails { get; set; } = "Tap 'Change' to select a child";
    public string ChildAvatar { get; set; } = "dotnet_bot.png";
    public string HealthScore { get; set; } = "--";
    public string HealthScoreLabel { get; set; } = "Health Score";
    public string Weight { get; set; } = "--";
    public string WeightLabel { get; set; } = "Last Weight";
    public string Level { get; set; } = "--";
    public string Points { get; set; } = "--";
    public int UnreadNotifications { get; set; } = 0;

    // Commands
    public ICommand LogoutCommand { get; }
    public ICommand ChangeChildCommand { get; }
    public ICommand RefreshCommand { get; }

    public async Task LoadDashboardAsync()
    {
        //if (IsLoading) return;

        try
        {
            IsLoading = true;
            ErrorMessage = "";

            // Get active child ID
            var activeChildId = await _activeChildStore.GetAsync();

            // Load dashboard from API
            _dashboardData = await _dashboardService.GetDashboardAsync(
                activeChildId: activeChildId,
                notificationsTake: 5,
                activitiesTake: 5);

            if (_dashboardData == null)
            {
                ErrorMessage = "Failed to load dashboard data";
                SetEmptyState();
                return;
            }

            // Check if we have an active child
            if (!_dashboardData.ActiveChildId.HasValue || _dashboardData.Children.Count == 0)
            {
                HasChild = false;
                SetEmptyState();
                return;
            }

            // Find the active child in the list
            var child = _dashboardData.Children.FirstOrDefault(c => c.Id == _dashboardData.ActiveChildId.Value);

            if (child == null)
            {
                HasChild = false;
                SetEmptyState();
                return;
            }

            // Update UI with active child data
            HasChild = true;
            ChildName = child.FullName;
            ChildDetails = $"{child.AgeYears} years old";
            ChildAvatar = string.IsNullOrWhiteSpace(child.AvatarUrl)
                ? "dotnet_bot.png"
                : child.AvatarUrl;

            // Level and Points are not in the API response, so hide them or set to default
            Level = child.Level.ToString();           // Now gets real data!
            Points = child.TotalPoints.ToString();    // Now gets real data!

            // Update health score
            if (_dashboardData.LatestHealthScore != null)
            {
                var score = _dashboardData.LatestHealthScore;
                HealthScore = score.TotalScore.ToString();  // Shows "25"
                HealthScoreLabel = $"Health Score ({score.TotalScore}/25)";
            }
            else
            {
                HealthScore = "--";
                HealthScoreLabel = "No Health Score Yet";
            }

            // Update weight/measurement
            if (_dashboardData.LatestMeasurement != null)
            {
                var measurement = _dashboardData.LatestMeasurement;
                Weight = $"{measurement.WeightKg:F1}kg";
                WeightLabel = !string.IsNullOrEmpty(measurement.CentileBand)
                    ? $"Weight ({measurement.CentileBand})"
                    : "Last Weight";
            }
            else
            {
                Weight = "--";
                WeightLabel = "No Measurements Yet";
            }

            // Notifications
            UnreadNotifications = _dashboardData.TotalUnreadNotifications;

            OnPropertyChanged(string.Empty); // Refresh all properties
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Network error: {ex.Message}";
            SetEmptyState();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            SetEmptyState();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void SetEmptyState()
    {
        HasChild = false;
        ChildName = "No child selected";
        ChildDetails = "Tap 'Change' to select a child";
        ChildAvatar = "dotnet_bot.png";
        HealthScore = "--";
        HealthScoreLabel = "Health Score";
        Weight = "--";
        WeightLabel = "Last Weight";
        Level = "--";
        Points = "--";
        UnreadNotifications = 0;
        OnPropertyChanged(string.Empty);
    }

    private async Task LogoutAsync()
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Logout",
            "Are you sure you want to logout?",
            "Yes",
            "No");

        if (!confirm) return;

        try
        {
            // Logout from server
            await _auth.LogoutAsync(allDevices: false);

            // Clear session
            _session.Clear();
            await _activeChildStore.ClearAsync();

            // Navigate to login
            var loginPage = App.ServiceProvider.GetRequiredService<LoginPage>();
            Application.Current.MainPage = loginPage;
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Logout Error", $"Could not logout: {ex.Message}", "OK");
        }
    }

    private async Task ChangeChildAsync()
    {
        await Shell.Current.GoToAsync("//ActiveChildPage");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}