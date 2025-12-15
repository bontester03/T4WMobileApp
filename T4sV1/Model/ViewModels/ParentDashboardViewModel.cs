using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using T4sV1.Model.Dashboard;
using T4sV1.Services.Http;
using T4sV1.Services.Security;
using T4sV1.Services;


namespace T4sV1.Model.ViewModels;

public sealed class ParentDashboardViewModel : INotifyPropertyChanged
{
    private readonly IDashboardService _dashboardService;
    private readonly IActiveChildStore _active;
    private readonly ISessionService _session;

    public ParentDashboardViewModel(
        IDashboardService dashboardService,
        IActiveChildStore active,
        ISessionService session)
    {
        _dashboardService = dashboardService;
        _active = active;
        _session = session;

        RefreshCommand = new Command(async () => await LoadAsync());
        BackCommand = new Command(async () => await Shell.Current.GoToAsync("//HomePage"));

        CreateRewardCommand = new Command<ChildSummaryDto>(async (child) => await CreateRewardAsync(child));
        ViewRewardsCommand = new Command<ChildSummaryDto>(async (child) => await ViewRewardsAsync(child));
        ApproveRequestsCommand = new Command<ChildSummaryDto>(async (child) => await ApproveRequestsAsync(child));
        AddTaskCommand = new Command<ChildSummaryDto>(async (child) => await AddTaskAsync(child));
        ViewTasksCommand = new Command<ChildSummaryDto>(async (child) => await ViewTasksAsync(child));
        AssignTaskCommand = new Command<ChildSummaryDto>(async (child) => await AssignTaskAsync(child));
        DeleteChildCommand = new Command<ChildSummaryDto>(async (child) => await DeleteChildAsync(child));
        AddChildCommand = new Command(async () => await AddChildAsync());
        ViewProfileCommand = new Command<ChildSummaryDto>(async (child) => await ViewProfileAsync(child));
    }

    public ObservableCollection<ChildSummaryDto> Children { get; } = new();

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public ICommand RefreshCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand CreateRewardCommand { get; }
    public ICommand ViewRewardsCommand { get; }
    public ICommand ApproveRequestsCommand { get; }
    public ICommand AddTaskCommand { get; }
    public ICommand ViewTasksCommand { get; }
    public ICommand AssignTaskCommand { get; }
    public ICommand DeleteChildCommand { get; }
    public ICommand AddChildCommand { get; }

    public async Task LoadAsync()
    {
        //if (IsBusy) return;

        try
        {
            IsBusy = true;
            System.Diagnostics.Debug.WriteLine("=== ParentDashboard LoadAsync started ===");

            var currentChildId = await _active.GetAsync();

            var dashboard = await _dashboardService.GetDashboardAsync(
                activeChildId: currentChildId,
                notificationsTake: 0,
                activitiesTake: 0).ConfigureAwait(false);

            if (dashboard == null)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Application.Current?.MainPage?.DisplayAlert("Error", "Failed to load children", "OK");
                });
                return;
            }

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Children.Clear();
                foreach (var child in dashboard.Children)
                {
                    Children.Add(child);
                }
                System.Diagnostics.Debug.WriteLine($"Loaded {Children.Count} children");
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current?.MainPage?.DisplayAlert("Error", $"Failed: {ex.Message}", "OK");
            });
        }
        finally
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = false;
            });
        }
    }

    private async Task CreateRewardAsync(ChildSummaryDto child)
    {
        // TODO: Navigate to create reward page
        await Application.Current.MainPage.DisplayAlert("Create Reward", $"Create reward for {child.FullName}", "OK");
    }

    private async Task ViewRewardsAsync(ChildSummaryDto child)
    {
        // TODO: Navigate to rewards page
        await Application.Current.MainPage.DisplayAlert("View Rewards", $"View rewards for {child.FullName}", "OK");
    }

    private async Task ApproveRequestsAsync(ChildSummaryDto child)
    {
        // TODO: Navigate to approve requests page
        await Application.Current.MainPage.DisplayAlert("Approve Requests", $"Approve requests for {child.FullName}", "OK");
    }

    private async Task AddTaskAsync(ChildSummaryDto child)
    {
        // TODO: Navigate to add task page
        await Application.Current.MainPage.DisplayAlert("Add Task", $"Add task for {child.FullName}", "OK");
    }

    private async Task ViewTasksAsync(ChildSummaryDto child)
    {
        // TODO: Navigate to tasks page
        await Application.Current.MainPage.DisplayAlert("View Tasks", $"View tasks for {child.FullName}", "OK");
    }

    private async Task AssignTaskAsync(ChildSummaryDto child)
    {
        // TODO: Navigate to assign task page
        await Application.Current.MainPage.DisplayAlert("Assign Task", $"Assign task to {child.FullName}", "OK");
    }

    private async Task DeleteChildAsync(ChildSummaryDto child)
    {
        var confirm = await Application.Current.MainPage.DisplayAlert(
            "Delete Child",
            $"Are you sure you want to delete {child.FullName}?",
            "Yes",
            "No");

        if (confirm)
        {
            // TODO: Implement delete child API call
            await Application.Current.MainPage.DisplayAlert("Delete", "Delete functionality coming soon", "OK");
        }
    }

    private async Task AddChildAsync()
    {
        // TODO: Navigate to add child page
        await Application.Current.MainPage.DisplayAlert("Add Child", "Add child functionality coming soon", "OK");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

    public ICommand ViewProfileCommand { get; }

    private async Task ViewProfileAsync(ChildSummaryDto child)
    {
        // Set this child as active and navigate to profile
        await _active.SetAsync(child.Id);
        _session.ActiveChildId = child.Id;
        _session.SaveToPreferences();

        await Shell.Current.GoToAsync("Profile");
    }
}