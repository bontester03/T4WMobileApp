using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.ObjectModel;
using T4sV1.Model.HealthScore;
using T4sV1.Services;
using T4sV1.Services.Http;

namespace T4sV1.Model.ViewModels;

public sealed class HealthScoreViewModel : INotifyPropertyChanged
{
    private readonly IHealthScoreService _healthScoreService;
    private readonly IActiveChildStore _activeChildStore;

    private bool _isLoading;
    private bool _hasData;
    private string _errorMessage = "";
    private HealthScoreDto? _latestHealthScore;

    public HealthScoreViewModel(
        IHealthScoreService healthScoreService,
        IActiveChildStore activeChildStore)
    {
        _healthScoreService = healthScoreService;
        _activeChildStore = activeChildStore;

        HealthScores = new ObservableCollection<HealthScoreDto>();

        RefreshCommand = new Command(async () => await LoadHealthScoresAsync());
        AddHealthScoreCommand = new Command(async () => await AddHealthScoreAsync());
        EditHealthScoreCommand = new Command<HealthScoreDto>(async (score) => await EditHealthScoreAsync(score));
        DeleteHealthScoreCommand = new Command<HealthScoreDto>(async (score) => await DeleteHealthScoreAsync(score));
        GoBackCommand = new Command(async () => await GoBackAsync());
    }

    // Properties
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public bool HasData
    {
        get => _hasData;
        set { _hasData = value; OnPropertyChanged(); OnPropertyChanged(nameof(NoDataAvailable)); }
    }

    public bool NoDataAvailable => !HasData;

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public HealthScoreDto? LatestHealthScore
    {
        get => _latestHealthScore;
        set { _latestHealthScore = value; OnPropertyChanged(); }
    }

    public ObservableCollection<HealthScoreDto> HealthScores { get; }

    // Display Properties for Latest Score
    public string TotalScoreDisplay => LatestHealthScore?.TotalScore.ToString() ?? "--";
    public string OverallScore10Display => LatestHealthScore?.OverallScore10.ToString("F1") ?? "--";
    public string PhysicalActivityDisplay => LatestHealthScore != null ? $"{LatestHealthScore.PhysicalActivityScore + 1}/5" : "--";
    public string BreakfastDisplay => LatestHealthScore != null ? $"{LatestHealthScore.BreakfastScore + 1}/5" : "--";
    public string FruitVegDisplay => LatestHealthScore != null ? $"{LatestHealthScore.FruitVegScore + 1}/5" : "--";
    public string SweetSnacksDisplay => LatestHealthScore != null ? $"{LatestHealthScore.SweetSnacksScore + 1}/5" : "--";
    public string FattyFoodsDisplay => LatestHealthScore != null ? $"{LatestHealthScore.FattyFoodsScore + 1}/5" : "--";
    public string DateRecordedDisplay => LatestHealthScore?.DateRecorded.ToString("MMM dd, yyyy") ?? "--";

    // Commands
    public ICommand RefreshCommand { get; }
    public ICommand AddHealthScoreCommand { get; }
    public ICommand EditHealthScoreCommand { get; }
    public ICommand DeleteHealthScoreCommand { get; }
    public ICommand GoBackCommand { get; }

    public async Task LoadHealthScoresAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = "";

            // Get active child ID
            var activeChildId = await _activeChildStore.GetAsync();

            if (!activeChildId.HasValue)
            {
                ErrorMessage = "No child selected. Please select a child first.";
                HasData = false;
                return;
            }

            // Load health scores from API
            var scores = await _healthScoreService.GetHealthScoresAsync(activeChildId.Value);

            if (scores == null || scores.Count == 0)
            {
                HasData = false;
                LatestHealthScore = null;
                HealthScores.Clear();
                return;
            }

            // Update collection
            HealthScores.Clear();
            foreach (var score in scores.OrderByDescending(s => s.DateRecorded))
            {
                HealthScores.Add(score);
            }

            // Set latest score
            LatestHealthScore = HealthScores.FirstOrDefault();
            HasData = true;

            // Update display properties
            OnPropertyChanged(nameof(TotalScoreDisplay));
            OnPropertyChanged(nameof(OverallScore10Display));
            OnPropertyChanged(nameof(PhysicalActivityDisplay));
            OnPropertyChanged(nameof(BreakfastDisplay));
            OnPropertyChanged(nameof(FruitVegDisplay));
            OnPropertyChanged(nameof(SweetSnacksDisplay));
            OnPropertyChanged(nameof(FattyFoodsDisplay));
            OnPropertyChanged(nameof(DateRecordedDisplay));
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Network error: {ex.Message}";
            HasData = false;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            HasData = false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task AddHealthScoreAsync()
    {
        try
        {
            var activeChildId = await _activeChildStore.GetAsync();
            if (!activeChildId.HasValue)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No child selected", "OK");
                return;
            }

            // Navigate to add health score page (you'll need to create this)
            await Shell.Current.GoToAsync("AddHealthScore");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to add health score: {ex.Message}", "OK");
        }
    }

    private async Task EditHealthScoreAsync(HealthScoreDto score)
    {
        if (score == null) return;

        try
        {
            // Navigate to edit page with the score ID
            await Shell.Current.GoToAsync($"EditHealthScore?scoreId={score.Id}");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to edit health score: {ex.Message}", "OK");
        }
    }

    private async Task DeleteHealthScoreAsync(HealthScoreDto score)
    {
        if (score == null) return;

        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Delete Health Score",
            $"Are you sure you want to delete the health score from {score.DateRecorded:MMM dd, yyyy}?",
            "Yes",
            "No");

        if (!confirm) return;

        try
        {
            IsLoading = true;
            bool success = await _healthScoreService.DeleteHealthScoreAsync(score.Id);

            if (success)
            {
                HealthScores.Remove(score);
                if (LatestHealthScore?.Id == score.Id)
                {
                    LatestHealthScore = HealthScores.FirstOrDefault();
                }
                HasData = HealthScores.Count > 0;

                await Application.Current.MainPage.DisplayAlert("Success", "Health score deleted successfully", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete health score", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}