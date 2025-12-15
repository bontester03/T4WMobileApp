using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.ObjectModel;
using T4sV1.Model.Measurement;
using T4sV1.Services;
using T4sV1.Services.Http;

namespace T4sV1.Model.ViewModels;

public sealed class MeasurementViewModel : INotifyPropertyChanged
{
    private readonly IMeasurementService _measurementService;
    private readonly IActiveChildStore _activeChildStore;

    private bool _isLoading;
    private bool _hasData;
    private string _errorMessage = "";
    private MeasurementDto? _latestMeasurement;

    public MeasurementViewModel(
        IMeasurementService measurementService,
        IActiveChildStore activeChildStore)
    {
        _measurementService = measurementService;
        _activeChildStore = activeChildStore;

        Measurements = new ObservableCollection<MeasurementDto>();

        RefreshCommand = new Command(async () => await LoadMeasurementsAsync());
        AddMeasurementCommand = new Command(async () => await AddMeasurementAsync());
        EditMeasurementCommand = new Command<MeasurementDto>(async (measurement) => await EditMeasurementAsync(measurement));
        DeleteMeasurementCommand = new Command<MeasurementDto>(async (measurement) => await DeleteMeasurementAsync(measurement));
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

    public MeasurementDto? LatestMeasurement
    {
        get => _latestMeasurement;
        set { _latestMeasurement = value; OnPropertyChanged(); }
    }

    public ObservableCollection<MeasurementDto> Measurements { get; }

    // Display Properties for Latest Measurement
    public string WeightDisplay => LatestMeasurement != null ? $"{LatestMeasurement.Weight:F1} kg" : "--";
    public string HeightDisplay => LatestMeasurement != null ? $"{LatestMeasurement.Height:F1} cm" : "--";
    public string BMIDisplay => LatestMeasurement?.BMI != null ? $"{LatestMeasurement.BMI:F1}" : "--";
    public string HealthRangeDisplay => LatestMeasurement?.HealthRange ?? "--";
    public string DateRecordedDisplay => LatestMeasurement?.DateRecorded.ToString("MMM dd, yyyy") ?? "--";

    // Commands
    public ICommand RefreshCommand { get; }
    public ICommand AddMeasurementCommand { get; }
    public ICommand EditMeasurementCommand { get; }
    public ICommand DeleteMeasurementCommand { get; }
    public ICommand GoBackCommand { get; }

    public async Task LoadMeasurementsAsync()
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

            // Load measurements from API
            var measurements = await _measurementService.GetMeasurementsAsync(activeChildId.Value);

            if (measurements == null || measurements.Count == 0)
            {
                HasData = false;
                LatestMeasurement = null;
                Measurements.Clear();
                return;
            }

            // Update collection
            Measurements.Clear();
            foreach (var measurement in measurements.OrderByDescending(m => m.DateRecorded))
            {
                Measurements.Add(measurement);
            }

            // Set latest measurement
            LatestMeasurement = Measurements.FirstOrDefault();
            HasData = true;

            // Update display properties
            OnPropertyChanged(nameof(WeightDisplay));
            OnPropertyChanged(nameof(HeightDisplay));
            OnPropertyChanged(nameof(BMIDisplay));
            OnPropertyChanged(nameof(HealthRangeDisplay));
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

    private async Task AddMeasurementAsync()
    {
        try
        {
            var activeChildId = await _activeChildStore.GetAsync();
            if (!activeChildId.HasValue)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No child selected", "OK");
                return;
            }

            // Navigate to add measurement page (you'll need to create this)
            await Shell.Current.GoToAsync("AddMeasurement");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to add measurement: {ex.Message}", "OK");
        }
    }

    private async Task EditMeasurementAsync(MeasurementDto measurement)
    {
        if (measurement == null) return;

        try
        {
            // Navigate to edit page with the measurement ID
            await Shell.Current.GoToAsync($"EditMeasurement?measurementId={measurement.Id}");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to edit measurement: {ex.Message}", "OK");
        }
    }

    private async Task DeleteMeasurementAsync(MeasurementDto measurement)
    {
        if (measurement == null) return;

        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Delete Measurement",
            $"Are you sure you want to delete the measurement from {measurement.DateRecorded:MMM dd, yyyy}?",
            "Yes",
            "No");

        if (!confirm) return;

        try
        {
            IsLoading = true;
            bool success = await _measurementService.DeleteMeasurementAsync(measurement.Id);

            if (success)
            {
                Measurements.Remove(measurement);
                if (LatestMeasurement?.Id == measurement.Id)
                {
                    LatestMeasurement = Measurements.FirstOrDefault();
                }
                HasData = Measurements.Count > 0;

                await Application.Current.MainPage.DisplayAlert("Success", "Measurement deleted successfully", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete measurement", "OK");
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