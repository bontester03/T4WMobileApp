using T4sV1.Model.ViewModels;

namespace T4sV1.Views;

public partial class Homepage : ContentPage
{
    private readonly HomePageViewModel _vm;

    public Homepage(HomePageViewModel viewModel)
    {
        InitializeComponent();
        _vm = viewModel;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Always reload dashboard when page appears (in case child was changed)
        await _vm.LoadDashboardAsync();
    }

    // Event handler for health score card tap
    private async void OnHealthScoreTapped(object sender, EventArgs e)
    {
        if (!_vm.HasChild)
        {
            await DisplayAlert("No Child Selected", "Please select a child first", "OK");
            return;
        }
        // TODO: Navigate to health score details
        await DisplayAlert("Health Score", "View detailed health score history", "OK");
    }

    // Event handler for measurements card tap
    private async void OnMeasurementsTapped(object sender, EventArgs e)
    {
        if (!_vm.HasChild)
        {
            await DisplayAlert("No Child Selected", "Please select a child first", "OK");
            return;
        }
        // TODO: Navigate to measurements history
        await DisplayAlert("Measurements", "View measurement history", "OK");
    }

    // Event handlers for actions that aren't implemented yet
    private async void OnAddHealthScoreTapped(object sender, EventArgs e)
    {
        if (!_vm.HasChild)
        {
            await DisplayAlert("No Child Selected", "Please select a child first", "OK");
            return;
        }

        try
        {
            await Shell.Current.GoToAsync("HealthScore");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to navigate: {ex.Message}", "OK");
        }
    }

    private async void OnAddMeasurementTapped(object sender, EventArgs e)
    {
        if (!_vm.HasChild)
        {
            await DisplayAlert("No Child Selected", "Please select a child first", "OK");
            return;
        }

        try
        {
            await Shell.Current.GoToAsync("Measurement");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to navigate: {ex.Message}", "OK");
        }
    }

    private async void OnViewResourcesTapped(object sender, EventArgs e)
    {
        // Navigate to Resources tab
        await Shell.Current.GoToAsync("//ResourcesPage");
    }
}