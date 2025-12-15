using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using T4sV1.Model.Dashboard;
using T4sV1.Services;
using T4sV1.Services.Http;
using T4sV1.Services.Security;

namespace T4sV1.Model.ViewModels;

public sealed class SelectActiveChildViewModel : INotifyPropertyChanged
{
    private readonly IDashboardService _dashboardService;
    private readonly IActiveChildStore _active;
    private readonly ISessionService _session;

    public SelectActiveChildViewModel(
        IDashboardService dashboardService,
        IActiveChildStore active,
        ISessionService session)
    {
        _dashboardService = dashboardService;
        _active = active;
        _session = session;

        SelectCommand = new Command<ChildSummaryDto>(async c => await SelectAsync(c));
        RefreshCommand = new Command(async () => await LoadAsync());
    }

    public ObservableCollection<ChildSummaryDto> Items { get; } = new();

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

    private int? _currentId;
    public int? CurrentId
    {
        get => _currentId;
        set
        {
            _currentId = value;
            OnPropertyChanged();
        }
    }

    public ICommand SelectCommand { get; }
    public ICommand RefreshCommand { get; }

    public async Task LoadAsync()
    {
        // Don't block if already loading - just return
        //if (IsBusy) return;

        string? errorMessage = null;

        try
        {
            IsBusy = true;
            System.Diagnostics.Debug.WriteLine("=== LoadAsync started ===");

            // Get current active child ID
            CurrentId = await _active.GetAsync();
            System.Diagnostics.Debug.WriteLine($"Active child ID: {CurrentId}");

            // Fetch dashboard data with ConfigureAwait to prevent deadlocks
            var dashboard = await _dashboardService.GetDashboardAsync(
                activeChildId: CurrentId,
                notificationsTake: 0,
                activitiesTake: 0).ConfigureAwait(false);

            System.Diagnostics.Debug.WriteLine($"Dashboard returned: {dashboard != null}");

            if (dashboard == null)
            {
                errorMessage = "Failed to load children data";
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Children count: {dashboard.Children?.Count ?? 0}");

            // Update the collection on the main thread
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Items.Clear();
                foreach (var child in dashboard.Children)
                {
                    Items.Add(child);
                }
                System.Diagnostics.Debug.WriteLine($"Items added: {Items.Count}");
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR: {ex.GetType().Name} - {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            errorMessage = $"Failed to load children: {ex.Message}";
        }
        finally
        {
            // Always set IsBusy to false on main thread
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = false;
            });

            // Show error after IsBusy is set to false
            if (!string.IsNullOrEmpty(errorMessage))
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", errorMessage, "OK");
                    }
                });
            }
        }
    }

    private async Task SelectAsync(ChildSummaryDto? child)
    {
        if (child is null) return;

        try
        {
            // Save to both stores
            await _active.SetAsync(child.Id);
            _session.ActiveChildId = child.Id;
            _session.SaveToPreferences();

            CurrentId = child.Id;

            // Navigate to Home tab
            await Shell.Current.GoToAsync("//HomePage");
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        $"Failed to select child: {ex.Message}",
                        "OK");
                }
            });
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
}