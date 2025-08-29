using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;
using T4sV1.Model;
using T4sV1.Services;

namespace T4sV1.Views;

public partial class WeeklyMeasurementsPage : ContentPage
{
    private readonly DashboardService _dashboardService = new();
    private readonly ISessionService _session;

    public ObservableCollection<MeasurementDto> Measurements { get; set; } = new();
    private string _selectedMetric = "Weight";

    public WeeklyMeasurementsPage()
    {
        InitializeComponent();

        _session = App.ServiceProvider.GetRequiredService<ISessionService>();
        _session.LoadFromPreferences();

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadChart("Weight"); // default chart

        try
        {
            if (!int.TryParse(_session.UserId, out int userId))
            {
                await DisplayAlert("Error", "User not logged in.", "OK");
                await Shell.Current.GoToAsync("//login");
                return;
            }

            var dashboard = await _dashboardService.GetDashboardOverviewAsync(userId);

            if (dashboard?.Measurements != null)
            {
                Measurements.Clear();
                foreach (var m in dashboard.Measurements)
                    Measurements.Add(m);
            }
            else
            {
                await DisplayAlert("No data", "No measurements found.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to load measurements.\n{ex.Message}", "OK");
        }
    }

    private void LoadChart(string metric)
    {
        MeasurementChart.Series.Clear();

        if (Measurements == null || Measurements.Count == 0)
            return;

        if (metric == "All")
        {
            MeasurementChart.Series.Add(CreateSeries(Measurements, "Weight", nameof(MeasurementDto.Weight)));
            MeasurementChart.Series.Add(CreateSeries(Measurements, "Height", nameof(MeasurementDto.Height)));
            MeasurementChart.Series.Add(CreateSeries(Measurements, "Centile", nameof(MeasurementDto.CentileScore)));
        }
        else
        {
            string yBinding = metric switch
            {
                "Weight" => nameof(MeasurementDto.Weight),
                "Height" => nameof(MeasurementDto.Height),
                "Centile" => nameof(MeasurementDto.CentileScore),
                _ => nameof(MeasurementDto.Weight)
            };

            MeasurementChart.Series.Add(CreateSeries(Measurements, metric, yBinding));
        }
    }

    private LineSeries CreateSeries(IEnumerable<MeasurementDto> data, string label, string yBindingPath)
    {
        return new LineSeries
        {
            ItemsSource = data,
            XBindingPath = nameof(MeasurementDto.DateRecorded),
            YBindingPath = yBindingPath,
            Label = label,
            ShowMarkers = true,
            EnableTooltip = true,
            StrokeWidth = 3
        };
    }
    private void OnMetricSelected(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string metric)
        {
            _selectedMetric = metric;
            LoadChart(metric);
        }
    }


}
